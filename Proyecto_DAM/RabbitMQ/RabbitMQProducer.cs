using System;
using System.Text;
using RabbitMQ.Client;

namespace Proyecto_DAM.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        void EnviarMensaje(string message);
    }

    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly string _queueName;
        private readonly string _hostname;

        public RabbitMQProducer(string queueName = "asignaturas", string hostname = "localhost")
        {
            _queueName = queueName;
            _hostname = hostname;
        }

        public void EnviarMensaje(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("El mensaje está vacío y no se enviará.");
                return;
            }

            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _queueName,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine($" [x] Mensaje enviado: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            }
        }
    }
}
