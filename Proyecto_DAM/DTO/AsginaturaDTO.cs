using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class AsignaturaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("creditos")]
        public int Creditos { get; set; }  

        [JsonPropertyName("idUsuario")]
        public string IdUsuario { get; set; }  

        public List<NotaDTO> Notas { get; set; }
        public List<EventoDTO> Eventos { get; set; }
    }

}
