using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class TiempoEstudioDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("asignaturaID")]
        public int AsignaturaID { get; set; }

        [JsonPropertyName("tiempoEstudiado")]
        public TimeSpan TiempoEstudiado { get; set; }
    }
}
