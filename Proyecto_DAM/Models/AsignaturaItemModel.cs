using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Models
{
    public class AsignaturaItemModel : ObservableObject
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Creditos { get; set; }
        public int Horas { get; set; }
        public int PorcentajeFaltas { get; set; }
        public int Faltas { get; set; }
        public int TotalEventos { get; set; }
        public int TotalNotas { get; set; }

        public static AsignaturaItemModel CreateModelFromDTO(AsignaturaDTO asignaturaDTO)
        {
            return new AsignaturaItemModel
            {
                Nombre = asignaturaDTO.Nombre,
                Id = asignaturaDTO.Id,
                Descripcion = asignaturaDTO.Descripcion,
                Horas = asignaturaDTO.Horas,
                PorcentajeFaltas = asignaturaDTO.PorcentajeFaltas,
                Faltas = asignaturaDTO.Faltas,
                Creditos = asignaturaDTO.Creditos,
                TotalEventos = 0,
                TotalNotas = 0
            };
        }
    }
}
