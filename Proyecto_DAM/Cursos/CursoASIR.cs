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
                    Creditos = 130,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Implantación de sistemas operativos propietarios",
                    Descripcion = "Implantación y configuración de sistemas operativos propietarios.",
                    Creditos = 130,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Planificación y administración de redes",
                    Descripcion = "Estudio de la planificación y administración de redes.",
                    Creditos = 190,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Fundamentos de hardware",
                    Descripcion = "Conceptos fundamentales de hardware de sistemas informáticos.",
                    Creditos = 95,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Gestión de bases de datos",
                    Descripcion = "Administración y gestión de bases de datos.",
                    Creditos = 160,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Lenguajes de marcas y sistemas de gestión de información",
                    Descripcion = "Estudio de lenguajes de marcas y gestión de la información.",
                    Creditos = 110,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Inglés técnico",
                    Descripcion = "Inglés aplicado a la informática.",
                    Creditos = 65,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de competencias laborales y orientación profesional.",
                    Creditos = 90,
                    IdUsuario = "admin"
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
                    Creditos = 140,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Servicios de red e Internet",
                    Descripcion = "Implementación y administración de servicios de red e Internet.",
                    Creditos = 140,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Implantación de aplicaciones Web",
                    Descripcion = "Desarrollo y puesta en marcha de aplicaciones web.",
                    Creditos = 100,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Administración de sistemas gestores de bases de datos",
                    Descripcion = "Administración de bases de datos y sus sistemas gestores.",
                    Creditos = 60,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Seguridad y alta disponibilidad",
                    Descripcion = "Implementación de sistemas de alta disponibilidad y seguridad.",
                    Creditos = 100,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Proyecto de Administración de sistemas informáticos en red",
                    Descripcion = "Desarrollo de un proyecto práctico de administración de sistemas informáticos.",
                    Creditos = 30,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Prácticas en empresas para aplicar los conocimientos adquiridos.",
                    Creditos = 400,
                    IdUsuario = "admin"
                }
            };
        }
    }
}
