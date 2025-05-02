using System.Text.Json.Serialization;

namespace Proyecto_DAM.DTO
{
    public class BienestarDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("estadoDeAnimo")]
        public string EstadoDeAnimo { get; set; }

        [JsonPropertyName("nivelDeEstres")]
        public int NivelDeEstres { get; set; }

        [JsonPropertyName("sugerencias")]
        public string Sugerencia { get; set; }
    }
}
