using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class VentaConsumerService : BackgroundService
    {
        private const string HostName = "localhost";
        private const string UserName = "sisventas";
        private const string Password = "ect*123";
        private const string ExchangeName = "ventas_exchange";

        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public VentaConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout, durable: true);

            var queueName = _channel.QueueDeclare(queue: "inventario_ventas_queue", durable: true, exclusive: false, autoDelete: false).QueueName;
            _channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonSerializer.Deserialize<VentaCreadaEvento>(json);
                    await ProcesarVentaCreada(evento);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando evento VentaCreada: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcesarVentaCreada(VentaCreadaEvento evento)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ContextDatabase>();

            // Crear el Egreso
            var egreso = new Egreso
            {
                Fecha = DateTime.Today,
                Glosa = $"Egreso por venta #{evento.VentaId}",
                Motivo = "venta",
                Estado = "activo",
                Id_Usuario = evento.UsuarioId
            };
            context.Egresos.Add(egreso);
            await context.SaveChangesAsync();

            // Crear cada DetalleEgreso (esto descuenta el stock automáticamente)
            foreach (var d in evento.Detalles)
            {
                var detalleEgreso = new DetalleEgreso
                {
                    Cantidad = (int)d.Cantidad,
                    Observacion = $"Venta #{evento.VentaId}",
                    Id_Egreso = egreso.Id,
                    Id_ArticuloAlmacen = d.IdProducto
                };
                context.DetallesEgreso.Add(detalleEgreso);

                var articuloAlmacen = await context.ArticulosAlmacenes
                    .FirstOrDefaultAsync(aa => aa.Id == d.IdProducto);
                if (articuloAlmacen != null)
                {
                    articuloAlmacen.Stock -= (int)d.Cantidad;
                    if (articuloAlmacen.Stock < 0) articuloAlmacen.Stock = 0;
                }
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Venta #{evento.VentaId} procesada correctamente en Inventario.");
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}