using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Cursos
{
    public class CursoDAW
    {
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasDAW2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Desarrollo web en entorno cliente",
                    Descripcion = "Diseño y desarrollo de aplicaciones web para el lado del cliente.",
                    Horas = 125,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Desarrollo web en entorno servidor",
                    Descripcion = "Desarrollo de aplicaciones web en el lado del servidor.",
                    Horas = 180,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Despliegue de aplicaciones web",
                    Descripcion = "Implantación y despliegue de aplicaciones web.",
                    Horas = 90,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Diseño de interfaces web",
                    Descripcion = "Creación de interfaces web eficientes y accesibles.",
                    Horas = 145,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Empresa e iniciativa emprendedora",
                    Descripcion = "Conocimiento de la creación y gestión de empresas.",
                    Horas = 60,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Practicas profesionales en una empresa.",
                    Horas = 400,
                    Creditos = 12
                },
                new AsignaturaDTO
                {
                    Nombre = "Proyecto de desarrollo de aplicaciones Web",
                    Descripcion = "Desarrollo de un proyecto práctico de DAW.",
                    Horas = 30,
                    Creditos = 12
                },
            };
        }
    }
}
