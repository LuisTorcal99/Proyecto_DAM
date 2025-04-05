using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models.Entity
{
    public class EventoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        
        [Required]
        public double Porcentaje { get; set; }

        public DateTime Fecha { get; set; }

        // Relación con Usuario
        [ForeignKey("User")]
        public string IdUsuario { get; set; }
        public User Usuario { get; set; }

        // Relación con Asignatura
        [ForeignKey("AsignaturaEntity")]
        public int IdAsignatura { get; set; }
        public AsignaturaEntity Asignatura { get; set; }

        // Relación uno a uno con Nota
        public NotaEntity Nota { get; set; }
    }
}
}
