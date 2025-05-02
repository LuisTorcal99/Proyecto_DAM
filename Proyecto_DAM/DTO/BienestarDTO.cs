using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    using System.Text.Json.Serialization;

    public class BienestarDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("estadoAnimo")]
        public int EstadoAnimo { get; set; }

        [JsonPropertyName("nivelEstres")]
        public int NivelEstres { get; set; }

        [JsonPropertyName("sugerencia")]
        public string Sugerencia { get; set; }

    }
}
