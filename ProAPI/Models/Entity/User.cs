using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models.Entity
{
    public class User
    {
        [Key]
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }

        // Relación uno a uno con AppUser (la tabla de Identity)
        public string AspNetUserId { get; set; }
        public AppUser AspNetUser { get; set; }

        // Relación con Asignaturas: un usuario puede tener muchas asignaturas
        public ICollection<AsignaturaEntity> Asignaturas { get; set; }

        // Relación con Eventos: un usuario puede tener muchos eventos
        public ICollection<EventoEntity> Eventos { get; set; }

        // Relación uno a muchos con Notas
        public ICollection<NotaEntity> Notas { get; set; }
    }
}
