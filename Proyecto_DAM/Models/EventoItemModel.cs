using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Models
{
    public class EventoItemModel
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public double Porcentaje { get; set; }
        public static EventoItemModel CreateModelFromDTO(EventoDTO eventoDTO)
        {
            return new EventoItemModel
            {
                Nombre = eventoDTO.Nombre,
                Descripcion = eventoDTO.Descripcion,
                Fecha = eventoDTO.Fecha,
                Porcentaje = eventoDTO.Porcentaje,
            };
        }
    }
}
