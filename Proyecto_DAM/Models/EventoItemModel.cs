using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using Proyecto_DAM.DTO;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.Models
{
    public class EventoItemModel
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public double Porcentaje { get; set; }
        public int IdAsignatura { get; set; }
        public string AsignaturaNombre { get; set; }

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

        public static EventoItemModel CreateModelFromDTO(EventoDTO eventoDTO, IEnumerable<AsignaturaDTO> asignaturas)
        {
            string asignaturaNombre = asignaturas
                .FirstOrDefault(a => a.Id == eventoDTO.IdAsignatura)?.Nombre ?? "Sin asignatura";

            return new EventoItemModel
            {
                Nombre = eventoDTO.Nombre,
                Tipo = eventoDTO.Tipo,
                Estado = eventoDTO.Estado,
                Fecha = eventoDTO.Fecha,
                Porcentaje = eventoDTO.Porcentaje,
                IdAsignatura = eventoDTO.IdAsignatura,
                AsignaturaNombre = asignaturaNombre
            };
        }
    }
}
