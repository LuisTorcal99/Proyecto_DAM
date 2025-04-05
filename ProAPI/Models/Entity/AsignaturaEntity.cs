using System.ComponentModel.DataAnnotations;

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

        public int IdUsuario { get; set; }
        public User Usuario { get; set; }

        public ICollection<NotaEntity> Notas { get; set; }

        public ICollection<EventoEntity> Eventos { get; set; }
    }
}
