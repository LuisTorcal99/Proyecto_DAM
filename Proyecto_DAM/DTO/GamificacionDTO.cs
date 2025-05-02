using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class GamificacionDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("tipoDeLogro")]
        public string TipoDeLogro { get; set; }

        [JsonPropertyName("puntos")]
        public int Puntos { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}
