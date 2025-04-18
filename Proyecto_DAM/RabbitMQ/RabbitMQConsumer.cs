using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
        private readonly IEmailSenderProvider _emailSender;

        public RabbitMQConsumer(IEmailSenderProvider emailSender, string queueName = "NuevaCola", string hostname = "localhost")
        {
            _queueName = queueName;
            _hostname = hostname;
            _emailSender = emailSender;
        }

        public async Task IniciarConsumo()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var mensajeRabbit = JsonSerializer.Deserialize<MensajeRabbit>(message);

                    if (mensajeRabbit != null)
                    {
                        if (mensajeRabbit.Tipo == "Email")
                        {
                            var email = JsonSerializer.Deserialize<Email>(mensajeRabbit.Contenido);
                            if (email != null)
                                await ProcesarEmail(email);
                        }
                        else if (mensajeRabbit.Tipo == "Evento")
                        {
                            await ProcesarEvento(mensajeRabbit.Contenido);
                        }
                        else
                        {
                            Console.WriteLine($"[!] Tipo de mensaje desconocido: {mensajeRabbit.Tipo}");
                        }
                    }
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

        private async Task ProcesarEmail(Email email)
        {
            await _emailSender.EnviarEmail(email);
            Console.WriteLine($"[✓] Email procesado para: {email.To}");
        }

        private async Task ProcesarEvento(string evento)
        {
            Console.WriteLine($"[🛈] Evento recibido: {evento}");
            await Task.CompletedTask;
        }
    }

    public class MensajeRabbit
    {
        public string Tipo { get; set; }
        public string Contenido { get; set; }
    }
}
