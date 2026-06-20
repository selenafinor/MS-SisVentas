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
    public class CompraConsumerService : BackgroundService
    {
        private const string HostName = "localhost";
        private const string UserName = "sisventas";
        private const string Password = "ect*123";
        private const string ExchangeName = "compras_exchange";

        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public CompraConsumerService(IServiceProvider serviceProvider)
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

            var queueName = _channel.QueueDeclare(queue: "inventario_compras_queue", durable: true, exclusive: false, autoDelete: false).QueueName;
            _channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonSerializer.Deserialize<CompraRegistradaEvento>(json);
                    await ProcesarCompraRegistrada(evento);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando evento CompraRegistrada: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcesarCompraRegistrada(CompraRegistradaEvento evento)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ContextDatabase>();

            // Crear el Ingreso
            var ingreso = new Ingreso
            {
                Fecha = DateTime.Today,
                Glosa = $"Ingreso por compra #{evento.CompraId}",
                Motivo = "compra",
                Estado = "activo",
                Id_Usuario = evento.UsuarioId
            };
            context.Ingresos.Add(ingreso);
            await context.SaveChangesAsync();

            // Crear cada DetalleIngreso (esto aumenta el stock)
            foreach (var d in evento.Detalles)
            {
                var detalleIngreso = new DetalleIngreso
                {
                    Cantidad = (int)d.Cantidad,
                    PrecioCompra = d.PrecioUni,
                    Observacion = $"Compra #{evento.CompraId}",
                    Id_Ingreso = ingreso.Id,
                    Id_ArticuloAlmacen = d.IdProducto
                };
                context.DetallesIngreso.Add(detalleIngreso);

                var articuloAlmacen = await context.ArticulosAlmacenes
                    .FirstOrDefaultAsync(aa => aa.Id == d.IdProducto);
                if (articuloAlmacen != null)
                {
                    articuloAlmacen.Stock += (int)d.Cantidad;
                }
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Compra #{evento.CompraId} procesada correctamente en Inventario.");
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}