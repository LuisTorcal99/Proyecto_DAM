using System.Net.Mail;
using System.Net;
using System.Text;
using Proyecto_DAM.DTO;
using RabbitMQ.Client;
using Proyecto_DAM.Interfaces;

namespace Proyecto_DAM.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        Task EnviarMensaje(string message);
        Task EnviarEmail(Email email);
    }

    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly string _queueName;
        private readonly string _hostname;
        private readonly IEmailSenderProvider _emailSender;

        public RabbitMQProducer(IEmailSenderProvider emailSender, string queueName = "NuevaCola", string hostname = "localhost")
        {
            _emailSender = emailSender;
            _queueName = queueName;
            _hostname = hostname;
        }

        public async Task EnviarEmail(Email email)
        {
            await _emailSender.EnviarEmail(email);
        }

        public async Task EnviarMensaje(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                Console.WriteLine($"Mensaje enviado: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            }
        }
    }
}