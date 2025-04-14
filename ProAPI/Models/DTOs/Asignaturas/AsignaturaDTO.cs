using RestAPI.Controllers;
using RestAPI.Models.DTOs.Evento;
using RestAPI.Models.DTOs.Notas;

namespace RestAPI.Models.DTOs.Asignaturas
{
    public class AsignaturaDTO : CreateAsignaturaDTO
    {
        public int Id { get; set; }
        public List<NotasDTO> Notas { get; set; }
        public List<EventoDTO> Eventos { get; set; }
    }
}
