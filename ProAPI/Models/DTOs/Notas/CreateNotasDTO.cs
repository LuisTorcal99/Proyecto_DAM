using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Notas
{
    public class CreateNotasDTO
    {
        [Required]
        public int NotaValor { get; set; }

        [Required]
        public int IdAsignatura { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required]
        public string IdUsuario { get; set; }
    }
}
