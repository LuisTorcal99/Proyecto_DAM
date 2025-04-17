using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;

namespace Proyecto_DAM.RabbitMQ
{
    public class EventoNotificacion
    {
        private readonly IRabbitMQProducer _rabbitMqProducer;
        private readonly IEventoApiProvider _eventoService;

        public EventoNotificacion(IRabbitMQProducer rabbitMqProducer, IEventoApiProvider eventoService)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _eventoService = eventoService;
        }

        public async Task VerificarYEnviarCorreos(List<AsignaturaDTO> asignaturas)
        {
            foreach (var asignatura in asignaturas)
            {
                foreach (var evento in asignatura.Eventos)
                {
                    if (evento.EmailEnviado)
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
        }

        private async Task EnviarCorreo(EventoDTO evento)
        {
            var email = new Email
            {
                To = App.Current.Services.GetService<LoginDTO>().Email,
                Subject = $"Recordatorio: Evento '{evento.Nombre}' en 2 días",
                Body = $"Este es un recordatorio de que el evento '{evento.Nombre}' ocurrirá el {evento.Fecha:dd/MM/yyyy}.",
                EventDate = evento.Fecha
            };

            await _rabbitMqProducer.EnviarEmail(email); 
            Console.WriteLine($"Correo enviado para el evento: {evento.Nombre}");
        }
    }
}
