using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Cursos
{
    public class CursoFarmaciaParafarmacia
    {
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasFarmacia1()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Disposición y venta de productos",
                    Descripcion = "Gestión de la venta y disposición de productos en farmacias.",
                    Horas = 125,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Oficina de farmacia",
                    Descripcion = "Gestión de una oficina de farmacia.",
                    Horas = 225,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Operaciones básicas de laboratorio",
                    Descripcion = "Técnicas básicas de laboratorio en farmacia.",
                    Horas = 315,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Primeros auxilios",
                    Descripcion = "Conocimientos y habilidades para la prestación de primeros auxilios.",
                    Horas = 65,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Anatomofisiología y patología básicas",
                    Descripcion = "Estudio de la anatomía y fisiología humana.",
                    Horas = 125,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de competencias laborales en el ámbito farmacéutico.",
                    Horas = 90,
                    Creditos = 6
                }
            };
        }
        public static ObservableCollection<AsignaturaDTO> ObtenerAsignaturasFarmacia2()
        {
            return new ObservableCollection<AsignaturaDTO>
            {
                new AsignaturaDTO
                {
                    Nombre = "Dispensación de productos farmacéuticos",
                    Descripcion = "Dispensación de medicamentos y productos farmacéuticos.",
                    Horas = 155,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Dispensación de productos parafarmacéuticos",
                    Descripcion = "Dispensación de productos parafarmacéuticos y productos de salud.",
                    Horas = 155,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formulación magistral",
                    Descripcion = "Elaboración de medicamentos personalizados en farmacia.",
                    Horas = 155,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Promoción de la salud",
                    Descripcion = "Estrategias para la promoción de la salud en la comunidad.",
                    Horas = 130,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Empresa e iniciativa emprendedora",
                    Descripcion = "Conocimientos sobre emprendimiento y gestión de empresas en el sector farmacéutico.",
                    Horas = 60,
                    Creditos = 6
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Prácticas en empresas farmacéuticas para aplicar los conocimientos adquiridos.",
                    Horas = 400,
                    Creditos = 6
                }
            };
        }
    }
}
