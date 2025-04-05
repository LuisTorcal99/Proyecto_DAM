using RestAPI.Controllers;

namespace RestAPI.Models.DTOs.Evento
{
    public class EventoDTO : CreateEventoDTO
    {
        public int Id { get; set; }
        public NotaDTO Nota { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
