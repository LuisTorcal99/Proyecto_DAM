using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class NotaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("calificacion")]
        public double Calificacion { get; set; }

        [JsonPropertyName("idAsignatura")]
        public int IdAsignatura { get; set; }

        [JsonPropertyName("idEvento")]
        public int IdEvento { get; set; }
    }
}
