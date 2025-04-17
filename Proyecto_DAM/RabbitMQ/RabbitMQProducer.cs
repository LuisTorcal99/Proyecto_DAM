using System.Net.Mail;
using System.Net;
using System.Text;
using Proyecto_DAM.DTO;
using RabbitMQ.Client;

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

        public RabbitMQProducer(string queueName = "NuevaCola", string hostname = "localhost")
        {
            _queueName = queueName;
            _hostname = hostname;
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

                Console.WriteLine($" [✓] Mensaje enviado: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] Error al enviar mensaje: {ex.Message}");
            }
        }

        public async Task EnviarEmail(Email email)
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
                Console.WriteLine($" [✓] Correo enviado a: {email.To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] Error al enviar correo: {ex.Message}");
            }
        }
    }
}
