using Proyecto_DAM.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Proyecto_DAM.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        Task IniciarConsumo();
        Task DetenerConsumo();
    }

    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly string _queueName;
        private readonly string _hostname;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(string queueName = "NuevaCola", string hostname = "localhost")
        {
            _queueName = queueName;
            _hostname = hostname;
        }

        public async Task IniciarConsumo()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var email = JsonSerializer.Deserialize<Email>(message);
                    if (email != null)
                        EnviarCorreo(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[✗] Error al procesar mensaje: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public async Task DetenerConsumo()
        {
            _channel?.Close();
            _connection?.Close();
        }

        private async Task EnviarCorreo(Email email)
        {
            try
            {
                var fromAddress = new MailAddress("correo@gmail.com", "NombreRemitente");
                var toAddress = new MailAddress(email.To);
                const string fromPassword = "contraseña";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = email.Subject,
                    Body = email.Body
                };

                smtp.Send(message);
                Console.WriteLine($"[✓] Correo enviado a: {email.To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] Error al enviar correo: {ex.Message}");
            }
        }
    }
}
