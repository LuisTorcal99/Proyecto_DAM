using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Models
{
    public class AsignaturaItemModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountNotas { get; set; }

        public static AsignaturaItemModel CreateModelFromDTO(AsignaturaDTO asignaturaDTO)
        {
            return new AsignaturaItemModel
            {
                Name = asignaturaDTO.Nombre,
                Description = asignaturaDTO.Descripcion,
                CountNotas = asignaturaDTO.Notas?.Count ?? 0,
            };
        }
    }
}
