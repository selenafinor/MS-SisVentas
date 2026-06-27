using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
namespace MSVenta.Compras.Services
{
    public class RabbitMqPublisher
    {
        private const string HostName = "localhost";
        private const string UserName = "sisventas";
        private const string Password = "ect*123";
        private const string ExchangeNameCompra = "compras_exchange";
        private const string ExchangeNameAdquisicion = "adquisicion_exchange";

        public void PublicarCompraRegistrada(object evento)
        {
            Publicar(evento, ExchangeNameCompra);
        }

        public void PublicarAdquisicionRegistrada(object evento)
        {
            Publicar(evento, ExchangeNameAdquisicion);
        }

        private void Publicar(object evento, string exchangeName)
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true);
            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: "",
                basicProperties: properties,
                body: body
            );
        }
    }
}