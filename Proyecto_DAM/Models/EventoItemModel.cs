using System;
using System.Windows.Media;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Models
{
    public class EventoItemModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int Porcentaje { get; set; }
        public DateTime FechaInicio { get; set; }

        public string IdUsuario { get; set; }
        public int IdAsignatura { get; set; }
        public NotaDTO Nota { get; set; }

        public Brush BackgroundColor
        {
            get
            {
                var diasRestantes = (Fecha - DateTime.Now).TotalDays;
                if (diasRestantes < 2) return Brushes.LightCoral;
                if (diasRestantes < 5) return Brushes.LightGoldenrodYellow;
                return Brushes.White;
            }
        }

        public static EventoItemModel CreateModelFromDTO(EventoDTO eventoDTO)
        {
            return new EventoItemModel
            {
                Id = eventoDTO.Id,
                Nombre = eventoDTO.Nombre,
                Tipo = eventoDTO.Tipo,
                Estado = eventoDTO.Estado,
                Fecha = eventoDTO.Fecha,
                Porcentaje = eventoDTO.Porcentaje,
                FechaInicio = eventoDTO.FechaInicio,
                IdUsuario = eventoDTO.IdUsuario,
                IdAsignatura = eventoDTO.IdAsignatura,
                Nota = eventoDTO.Nota
            };
        }

        public EventoDTO ToDTO()
        {
            return new EventoDTO
            {
                Id = this.Id,
                Nombre = this.Nombre,
                Tipo = this.Tipo,
                Estado = this.Estado,
                Fecha = this.Fecha,
                Porcentaje = this.Porcentaje,
                FechaInicio = this.FechaInicio,
                IdUsuario = this.IdUsuario,
                IdAsignatura = this.IdAsignatura,
                Nota = this.Nota
            };
        }
    }
}
