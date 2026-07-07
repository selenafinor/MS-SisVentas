using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace MSVenta.Venta.Services
{
    public class RabbitMqPublisher
    {
        private static readonly string HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private const string UserName = "sisventas";
        private const string Password = "ect*123";
        private const string ExchangeName = "ventas_exchange";

        public void PublicarVentaCreada(object evento)
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout, durable: true);

            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "",
                basicProperties: properties,
                body: body
            );
        }
    }
}