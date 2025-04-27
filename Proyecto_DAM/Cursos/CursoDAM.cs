using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.Cursos
{
    public class CursoDAM
    {
        // Asignaturas de 1º DAM
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasDAMDAW1()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Sistemas informáticos",
                    Descripcion = "Fundamentos de los sistemas informáticos.",
                    Creditos = 170,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Bases de datos",
                    Descripcion = "Introducción a las bases de datos relacionales.",
                    Creditos = 190,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación",
                    Descripcion = "Asignatura de programación básica en Java.",
                    Creditos = 245, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Lenguajes de marcas y sistemas de gestión de información",
                    Descripcion = "Trabajo con lenguajes de marcas y sistemas de gestión de información.",
                    Creditos = 110, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Entorno de desarrollo",
                    Descripcion = "Configuración y uso de entornos de desarrollo de software.",
                    Creditos = 100, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Inglés técnico",
                    Descripcion = "Estudio del inglés técnico aplicado a la informática.",
                    Creditos = 65,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de habilidades y conocimientos para el entorno laboral.",
                    Creditos = 90, 
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 2º DAM
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasDAM2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Acceso a datos",
                    Descripcion = "Manejo y acceso a bases de datos desde aplicaciones.",
                    Creditos = 125, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Desarrollo de interfaces",
                    Descripcion = "Creación de interfaces de usuario para aplicaciones.",
                    Creditos = 140, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación multimedia y dispositivos móviles",
                    Descripcion = "Desarrollo de aplicaciones multimedia y para dispositivos móviles.",
                    Creditos = 85, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación de servicios y procesos",
                    Descripcion = "Creación de servicios y procesos en sistemas operativos.",
                    Creditos = 70, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Sistemas de gestión empresarial",
                    Descripcion = "Implantación de sistemas informáticos de gestión empresarial.",
                    Creditos = 120, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Empresa e iniciativa emprendedora",
                    Descripcion = "Conocimiento de la creación y gestión de empresas.",
                    Creditos = 60, 
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Proyecto de desarrollo de aplicaciones multiplataforma",
                    Descripcion = "Desarrollo de un proyecto práctico de DAM.",
                    Creditos = 30,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Practicas profesionales en una empresa.",
                    Creditos = 400,
                    IdUsuario = "admin"
                }
            };
        }
    }
}