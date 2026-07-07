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
    public class AdquisicionConsumerService : BackgroundService
    {
        private static readonly string HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private const string UserName = "sisventas";
        private const string Password = "ect*123";
        private const string ExchangeName = "adquisicion_exchange";

        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public AdquisicionConsumerService(IServiceProvider serviceProvider)
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

            var queueName = _channel.QueueDeclare(queue: "inventario_adquisicion_queue", durable: true, exclusive: false, autoDelete: false).QueueName;
            _channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonSerializer.Deserialize<AdquisicionRegistradaEvento>(json);
                    await ProcesarAdquisicionRegistrada(evento);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando evento AdquisicionRegistrada: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcesarAdquisicionRegistrada(AdquisicionRegistradaEvento evento)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ContextDatabase>();

            foreach (var d in evento.Detalles)
            {
                var producto = await context.Articulos.FirstOrDefaultAsync(a => a.Id == d.IdProducto);
                if (producto == null) continue;

                // Buscar o crear el ArticuloAlmacen (producto + almacén)
                var articuloAlmacen = await context.ArticulosAlmacenes
                    .FirstOrDefaultAsync(aa => aa.Id_Articulo == d.IdProducto && aa.Id_Almacen == d.IdAlmacen);

                if (articuloAlmacen == null)
                {
                    articuloAlmacen = new ArticuloAlmacen
                    {
                        Id_Articulo = d.IdProducto,
                        Id_Almacen = d.IdAlmacen,
                        Stock = 0,
                        StockMin = 0,
                        StockMax = 0
                    };
                    context.ArticulosAlmacenes.Add(articuloAlmacen);
                    await context.SaveChangesAsync();
                }

                // Calcular precio promedio ponderado
                decimal stockActual = articuloAlmacen.Stock;
                decimal precioActual = producto.Precio;
                decimal precioNuevo = d.PrecioUni;
                decimal precioPromedio;

                if (stockActual > 0 && precioActual > 0)
                {
                    precioPromedio = (stockActual * precioActual + d.Cantidad * precioNuevo) / (stockActual + d.Cantidad);
                }
                else
                {
                    precioPromedio = precioNuevo;
                }

                producto.Precio = precioPromedio;

                context.GesPrecios.Add(new GesPrecio
                {
                    Id_Articulo = producto.Id,
                    PrecioCompra = precioNuevo,
                    PrecioVenta = precioPromedio,
                    Fecha = DateTime.Today,
                    MetodoInventario = "PROMEDIO"
                });

                articuloAlmacen.Stock += (int)d.Cantidad;
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Adquisición #{evento.AdquisicionId} procesada correctamente en Inventario (stock + precio actualizados).");
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}