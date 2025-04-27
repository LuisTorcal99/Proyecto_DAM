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
                    Creditos = 125,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Oficina de farmacia",
                    Descripcion = "Gestión de una oficina de farmacia.",
                    Creditos = 225,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Operaciones básicas de laboratorio",
                    Descripcion = "Técnicas básicas de laboratorio en farmacia.",
                    Creditos = 315,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Primeros auxilios",
                    Descripcion = "Conocimientos y habilidades para la prestación de primeros auxilios.",
                    Creditos = 65,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Anatomofisiología y patología básicas",
                    Descripcion = "Estudio de la anatomía y fisiología humana.",
                    Creditos = 125,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación y Orientación Laboral",
                    Descripcion = "Desarrollo de competencias laborales en el ámbito farmacéutico.",
                    Creditos = 90,
                    IdUsuario = "admin"
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
                    Creditos = 155,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Dispensación de productos parafarmacéuticos",
                    Descripcion = "Dispensación de productos parafarmacéuticos y productos de salud.",
                    Creditos = 155,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formulación magistral",
                    Descripcion = "Elaboración de medicamentos personalizados en farmacia.",
                    Creditos = 155,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Promoción de la salud",
                    Descripcion = "Estrategias para la promoción de la salud en la comunidad.",
                    Creditos = 130,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Empresa e iniciativa emprendedora",
                    Descripcion = "Conocimientos sobre emprendimiento y gestión de empresas en el sector farmacéutico.",
                    Creditos = 60,
                    IdUsuario = "admin"
                },
                new AsignaturaDTO
                {
                    Nombre = "Formación en Centros de Trabajo",
                    Descripcion = "Prácticas en empresas farmacéuticas para aplicar los conocimientos adquiridos.",
                    Creditos = 400,
                    IdUsuario = "admin"
                }
            };
        }
    }
}
