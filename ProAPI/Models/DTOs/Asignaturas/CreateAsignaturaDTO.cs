using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Asignaturas
{
    public class CreateAsignaturaDTO
    {
        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public int Creditos { get; set; }

        public int Horas { get; set; }

        public int PorcentajeFaltas { get; set; }

        public int Faltas { get; set; }

        [Required]
        public int IdUsuario { get; set; }
    }
}
