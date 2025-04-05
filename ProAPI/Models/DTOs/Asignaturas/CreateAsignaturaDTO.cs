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

        [Required]
        public string IdUsuario { get; set; }
    }
}
