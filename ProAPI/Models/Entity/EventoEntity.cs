using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models.Entity
{
    public class EventoEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Porcentaje { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }

        // Relación con el usuario
        public string IdUsuario { get; set; }
        public User Usuario { get; set; }

        // Relación con la asignatura
        public int IdAsignatura { get; set; }
        public AsignaturaEntity Asignatura { get; set; }

        // Relación uno a uno con Nota
        public NotaEntity Nota { get; set; }
    }

}