using System.Text.Json.Serialization;

namespace Proyecto_DAM.DTO
{
    public class NotaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("notaValor")]
        public double NotaValor { get; set; }

        [JsonPropertyName("idAsignatura")]
        public int IdAsignatura { get; set; }

        [JsonPropertyName("idEvento")]
        public int IdEvento { get; set; }

        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
    }
}
