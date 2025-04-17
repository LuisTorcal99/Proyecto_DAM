using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Proyecto_DAM.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        void IniciarConsumo();
        void DetenerConsumo();
    }

    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly string _queueName;
        private readonly string _hostname;

        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(string queueName = "asignaturas", string hostname = "localhost")
        {
            _queueName = queueName;
            _hostname = hostname;
        }

        public void IniciarConsumo()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"[✓] Mensaje recibido: {message}");
            };

            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void DetenerConsumo()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
