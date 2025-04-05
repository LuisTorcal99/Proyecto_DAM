using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models.Entity
{
    public class AsignaturaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Creditos { get; set; }

        // Relación con Usuario
        [ForeignKey("User")]
        public string IdUsuario { get; set; }
        public User Usuario { get; set; }

        // Relación uno a muchos con Notas
        public ICollection<NotaEntity> Notas { get; set; }

        // Relación uno a muchos con Eventos
        public ICollection<EventoEntity> Eventos { get; set; }
    }
}
