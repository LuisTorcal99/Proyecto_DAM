using System.Collections.ObjectModel;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Cursos
{
    public class CursoIngenieriaInformatica
    {
        // Asignaturas de 1º Curso de Ingeniería Informática
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasIngenieriaInformática1()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Matemáticas I",
                    Descripcion = "Fundamentos matemáticos aplicados a la informática.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Física I",
                    Descripcion = "Fundamentos de la física para la ingeniería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Informática I",
                    Descripcion = "Conceptos básicos de la informática y la programación.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Álgebra Lineal",
                    Descripcion = "Estudio de matrices, vectores y sistemas de ecuaciones lineales.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Estructuras de Datos",
                    Descripcion = "Estudio de las estructuras de datos más utilizadas en programación.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Física II",
                    Descripcion = "Continuación de la asignatura de Física I, enfocada en la termodinámica y electromagnetismo.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 2º Curso de Ingeniería Informática
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasIngenieriaInformática2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Matemáticas II",
                    Descripcion = "Cálculo diferencial e integral aplicado a la ingeniería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación II",
                    Descripcion = "Ampliación de conceptos en programación, con énfasis en programación orientada a objetos.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Sistemas Operativos",
                    Descripcion = "Estudio de los sistemas operativos y sus mecanismos de funcionamiento.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Teoría de la Computación",
                    Descripcion = "Fundamentos teóricos de la computación y la automatización.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Bases de Datos",
                    Descripcion = "Diseño y gestión de bases de datos relacionales.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 3º Curso de Ingeniería Informática
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasIngenieriaInformática3()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Arquitectura de Computadores",
                    Descripcion = "Estudio de la arquitectura interna de las computadoras y su funcionamiento.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Redes de Computadores",
                    Descripcion = "Diseño y gestión de redes de comunicación de datos.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Inteligencia Artificial",
                    Descripcion = "Fundamentos y aplicaciones de la inteligencia artificial en informática.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Desarrollo de Software",
                    Descripcion = "Metodologías y herramientas para el desarrollo de software.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación Concurrente",
                    Descripcion = "Técnicas y enfoques para desarrollar aplicaciones concurrentes.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 4º Curso de Ingeniería Informática
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasIngenieriaInformática4()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Gestión de Proyectos Informáticos",
                    Descripcion = "Metodologías y herramientas para la gestión eficiente de proyectos tecnológicos.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Seguridad Informática",
                    Descripcion = "Estudio de los sistemas y protocolos de seguridad en la informática.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Sistemas Distribuidos",
                    Descripcion = "Estudio de arquitecturas y sistemas distribuidos para aplicaciones avanzadas.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Programación de Aplicaciones Web",
                    Descripcion = "Diseño y desarrollo de aplicaciones web interactivas.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Prácticas Externas",
                    Descripcion = "Aplicación de los conocimientos adquiridos en entornos profesionales.",
                    Creditos = 12,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Trabajo Fin de Grado",
                    Descripcion = "Desarrollo de un trabajo de investigación o de aplicación en el área de la informática.",
                    Creditos = 12,
                    IdUsuario = "admin"
                }
            };
        }
    }
}
