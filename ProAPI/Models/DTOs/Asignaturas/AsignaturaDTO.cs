using RestAPI.Controllers;
using RestAPI.Models.DTOs.Evento;

namespace RestAPI.Models.DTOs.Asignaturas
{
    public class AsignaturaDTO : CreateAsignaturaDTO
    {
        public int Id { get; set; }
        public List<NotaDTO> Notas { get; set; }
        public List<EventoDTO> Eventos { get; set; }
    }
}
