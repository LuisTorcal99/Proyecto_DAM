using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Evento
{
    public class CreateEventoDTO
    {
        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public double Porcentaje { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdAsignatura { get; set; }

        [Required]
        public string Tipo { get; set; }   // "Tarea" o "Examen"

        [Required]
        public string Estado { get; set; } // "Pendiente", "EnProceso", "Realizado"
        public bool EmailEnviado { get; set; }
    }
}
