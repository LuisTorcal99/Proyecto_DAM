using System.Collections.ObjectModel;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Cursos
{
    public class CursoEnfermeria
    {
        // Asignaturas de 1º Curso de Enfermería
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasEnfermeria1()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Anatomía humana",
                    Descripcion = "Estudio de la anatomía del cuerpo humano.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Fisiología I",
                    Descripcion = "Estudio de la fisiología humana en sus primeros niveles.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Psicología I",
                    Descripcion = "Estudio de los procesos psicológicos básicos.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Informática",
                    Descripcion = "Uso de herramientas informáticas aplicadas a la enfermería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Fundamentos de enfermería",
                    Descripcion = "Conceptos básicos y principios de la enfermería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Fisiología II",
                    Descripcion = "Estudio de la fisiología humana avanzada.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Bioquímica",
                    Descripcion = "Estudio de la bioquímica aplicada a la salud.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería comunitaria I",
                    Descripcion = "Enfermería en el contexto de la salud comunitaria.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería médico quirúrgica I",
                    Descripcion = "Cuidados de enfermería en el ámbito médico-quirúrgico.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Historia de la enfermería",
                    Descripcion = "Estudio de la evolución histórica de la profesión de enfermería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 2º Curso de Enfermería
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasEnfermeria2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Psicología II",
                    Descripcion = "Estudio de la psicología aplicada a la salud.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Farmacología I",
                    Descripcion = "Introducción a la farmacología y sus aplicaciones.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería comunitaria II",
                    Descripcion = "Avances y aplicación de la enfermería comunitaria.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería ciclo vital: enfermería materno infantil",
                    Descripcion = "Enfermería aplicada en el ciclo vital materno-infantil.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Ética y legislación",
                    Descripcion = "Estudio de la ética y las leyes que rigen la profesión de enfermería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Nutrición y dietética",
                    Descripcion = "Fundamentos de la nutrición y su relación con la salud.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Farmacología II",
                    Descripcion = "Estudio avanzado de la farmacología.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería médico-quirúrgica II",
                    Descripcion = "Enfermería en el ámbito médico-quirúrgico avanzado.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }

        // Asignaturas de 3º Curso de Enfermería
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasEnfermeria3()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Enfermería comunitaria III",
                    Descripcion = "Aplicación de los cuidados de enfermería en la comunidad.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Fundamentos de gestión en enfermería",
                    Descripcion = "Gestión de los recursos en el ámbito de la enfermería.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Diagnóstico por imagen. Cuidados enfermeros",
                    Descripcion = "Estudio de la relación entre diagnóstico por imagen y cuidados enfermeros.",
                    Creditos = 3,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería médico quirúrgica III",
                    Descripcion = "Cuidados enfermeros en el ámbito quirúrgico avanzado.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Enfermería ciclo vital: enfermería geriátrica",
                    Descripcion = "Cuidados enfermeros en el ciclo vital geriátrico.",
                    Creditos = 6,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Salud mental",
                    Descripcion = "Cuidados de enfermería en el ámbito de la salud mental.",
                    Creditos = 6,
                    IdUsuario = "admin"
                }
            };
        }
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasEnfermeria4()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Trabajo fin de grado en Enfermería",
                    Descripcion = "Desarrollo del proyecto final que evalúa el aprendizaje global de la carrera.",
                    Creditos = 12,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Prácticas clínicas IV",
                    Descripcion = "Aplicación práctica de los conocimientos adquiridos durante la carrera.",
                    Creditos = 24,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Prácticas clínicas V",
                    Descripcion = "Fase final de las prácticas clínicas de la formación en enfermería.",
                    Creditos = 18,
                    IdUsuario = "admin"
                }
            };
        }
    }
}
