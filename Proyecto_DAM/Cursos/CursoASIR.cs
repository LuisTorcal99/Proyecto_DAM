using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Cursos
{
    public class CursoASIR
    {
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasASIR1()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Implantación de sistemas operativos libres",
                    Descripcion = "Implantación y configuración de sistemas operativos libres.",
                    Horas = 130,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Implantación de sistemas operativos propietarios",
                    Descripcion = "Implantación y configuración de sistemas operativos propietarios.",
                    Horas = 130,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Planificación y administración de redes",
                    Descripcion = "Estudio de la planificación y administración de redes.",
                    Horas = 190,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Fundamentos de hardware",
                    Descripcion = "Conceptos fundamentales de hardware de sistemas informáticos.",
                    Horas = 95,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Gestión de bases de datos",
                    Descripcion = "Administración y gestión de bases de datos.",
                    Horas = 160,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Lenguajes de marcas y sistemas de gestión de información",
                    Descripcion = "Estudio de lenguajes de marcas y gestión de la información.",
                    Horas = 110,
                    Creditos = 6,
                },
                new AsignaturaDTO
                {
                    Nombre = "Inglés técnico",
                    Descripcion = "Inglés aplicado a la informática.",
                    Horas = 65,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de competencias laborales y orientación profesional.",
                    Horas = 90,
                    Creditos = 6
                }
            };
        }
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasASIR2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Administración de sistemas operativos",
                    Descripcion = "Administración de sistemas operativos en entornos profesionales.",
                    Horas = 140,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Servicios de red e Internet",
                    Descripcion = "Implementación y administración de servicios de red e Internet.",
                    Horas = 140,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Implantación de aplicaciones Web",
                    Descripcion = "Desarrollo y puesta en marcha de aplicaciones web.",
                    Horas = 100,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Administración de sistemas gestores de bases de datos",
                    Descripcion = "Administración de bases de datos y sus sistemas gestores.",
                    Horas = 60,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Seguridad y alta disponibilidad",
                    Descripcion = "Implementación de sistemas de alta disponibilidad y seguridad.",
                    Horas = 100,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Proyecto de Administración de sistemas informáticos en red",
                    Descripcion = "Desarrollo de un proyecto práctico de administración de sistemas informáticos.",
                    Horas = 30,
                    Creditos = 12
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Prácticas en empresas para aplicar los conocimientos adquiridos.",
                    Horas = 400,
                    Creditos = 12
                }
            };
        }
    }
}
