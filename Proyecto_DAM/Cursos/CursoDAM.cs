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
                    Horas = 170,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Bases de datos",
                    Descripcion = "Introducción a las bases de datos relacionales.",
                    Horas = 190,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación",
                    Descripcion = "Asignatura de programación básica en Java.",
                    Horas = 245,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Lenguajes de marcas y sistemas de gestión de información",
                    Descripcion = "Trabajo con lenguajes de marcas y sistemas de gestión de información.",
                    Horas = 110,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Entorno de desarrollo",
                    Descripcion = "Configuración y uso de entornos de desarrollo de software.",
                    Horas = 100,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Inglés técnico",
                    Descripcion = "Estudio del inglés técnico aplicado a la informática.",
                    Horas = 65,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de habilidades y conocimientos para el entorno laboral.",
                    Horas = 90,
                    Creditos = 6
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
                    Horas = 125,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Desarrollo de interfaces",
                    Descripcion = "Creación de interfaces de usuario para aplicaciones.",
                    Horas = 140,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación multimedia y dispositivos móviles",
                    Descripcion = "Desarrollo de aplicaciones multimedia y para dispositivos móviles.",
                    Horas = 85,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación de servicios y procesos",
                    Descripcion = "Creación de servicios y procesos en sistemas operativos.",
                    Horas = 70,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Sistemas de gestión empresarial",
                    Descripcion = "Implantación de sistemas informáticos de gestión empresarial.",
                    Horas = 120,
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
                    Nombre = "Proyecto de desarrollo de aplicaciones multiplataforma",
                    Descripcion = "Desarrollo de un proyecto práctico de DAM.",
                    Horas = 30,
                    Creditos = 12
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Practicas profesionales en una empresa.",
                    Horas = 400,
                    Creditos = 12
                }
            };
        }
    }
}