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

        [JsonPropertyName("porcentaje")]
        public int Porcentaje { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonPropertyName("idUsuario")]
        public string IdUsuario { get; set; }

        [JsonPropertyName("idAsignatura")]
        public int IdAsignatura { get; set; }

        [JsonPropertyName("nota")]
        public NotaDTO? Nota { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }   // "Tarea" o "Examen"

        [JsonPropertyName("estado")]
        public string Estado { get; set; } // "Pendiente", "EnProceso", "Realizado"
    }
}
