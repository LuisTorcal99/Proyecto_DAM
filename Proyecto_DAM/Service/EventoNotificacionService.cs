using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;

namespace Proyecto_DAM.Service
{
    public class EventoNotificacionService : IEventoNotificacionProvider
    {
        private readonly IRabbitMQProducer _rabbitMqProducer;
        private readonly IEventoApiProvider _eventoService;


        public EventoNotificacionService(IRabbitMQProducer rabbitMqProducer, IEventoApiProvider eventoService)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _eventoService = eventoService;
        }
        public async Task VerificarYEnviarCorreos(List<EventoDTO> eventos, List<NotaDTO> notas, int idUsuario)
        {
            foreach (var evento in eventos)
            {
                var nota = notas.FirstOrDefault(n => n.IdEvento == evento.Id);

                if (nota != null)
                {
                    evento.Nota = nota; 
                }
                else
                {
                    evento.Nota = new NotaDTO
                    {
                        NotaValor = -1, 
                        IdEvento = evento.Id,
                        IdAsignatura = evento.IdAsignatura, 
                        IdUsuario = idUsuario
                    };
                }

                if (evento.EmailEnviado || evento.Estado.Equals("Completado"))
                    continue;

                var diasRestantes = (evento.Fecha - DateTime.Now).TotalDays;

                if (diasRestantes <= 2 && diasRestantes > 0)
                {
                    await EnviarCorreo(evento);
                    evento.EmailEnviado = true;
                    await _eventoService.PatchEvento(evento);
                }
            }
        }
        private async Task EnviarCorreo(EventoDTO evento)
        {
            var email = new Email
            {
                To = App.Current.Services.GetService<LoginDTO>().Email,
                Subject = $"Recordatorio: Evento '{evento.Nombre}' en menos de 2 días",
                Body = $"Este es un recordatorio de que el evento '{evento.Nombre}' ocurrirá el {evento.Fecha:dd/MM/yyyy} a las {evento.Fecha:HH:mm}.",
            };

            var mensaje = new MensajeRabbit
            {
                Tipo = "Email",
                Contenido = JsonSerializer.Serialize(email)
            };

            await _rabbitMqProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
            Console.WriteLine($"Correo enviado para el evento: {evento.Nombre} a las {evento.Fecha:HH:mm}");
        }
    }
}
