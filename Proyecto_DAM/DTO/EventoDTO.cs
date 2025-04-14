using System;
using System.Text.Json.Serialization;

namespace Proyecto_DAM.DTO
{
    public class EventoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("porcentaje")]
        public double Porcentaje { get; set; }

        [JsonPropertyName("idUsuario")]
        public string IdUsuario { get; set; }

        [JsonPropertyName("idAsignatura")]
        public int IdAsignatura { get; set; }
    }
}
