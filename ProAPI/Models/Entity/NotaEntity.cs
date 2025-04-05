using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Entity
{
    public class NotaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NotaValor { get; set; }

        public int IdAsignatura { get; set; }
        public AsignaturaEntity Asignatura { get; set; }

        public int IdEvento { get; set; }
        public EventoEntity Evento { get; set; }

        public int IdUsuario { get; set; }
        public User Usuario { get; set; }
    }
}
