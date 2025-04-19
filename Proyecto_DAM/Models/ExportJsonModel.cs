using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.Models
{
    public class ExportJsonModel
    {
        public class Asignatura
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public int Creditos { get; set; }
            public List<Evento> Eventos { get; set; }
        }

        public class Evento
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public DateTime Fecha { get; set; }
            public double Porcentaje { get; set; }
            public string Tipo { get; set; }
            public string Estado { get; set; }
            public double? Nota { get; set; }
        }
    }
}
