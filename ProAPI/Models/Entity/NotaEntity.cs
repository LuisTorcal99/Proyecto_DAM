using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Entity
{
    public class NotaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NotaValor { get; set; }

        // Relación con Asignatura
        [ForeignKey("AsignaturaEntity")]
        public int IdAsignatura { get; set; }
        public AsignaturaEntity Asignatura { get; set; }

        // Relación con Evento
        [ForeignKey("EventoEntity")]
        public int IdEvento { get; set; }
        public EventoEntity Evento { get; set; }

        // Relación con Usuario
        [ForeignKey("User")]
        public string IdUsuario { get; set; }
        public User Usuario { get; set; }
    }

}
