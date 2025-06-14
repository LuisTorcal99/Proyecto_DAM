﻿using System.Text.Json.Serialization;

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
        public double Porcentaje { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("idAsignatura")]
        public int IdAsignatura { get; set; }

        [JsonPropertyName("nota")]
        public double? Nota { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }   // "Nota", "Tarea" o "Examen"

        [JsonPropertyName("estado")]
        public string Estado { get; set; } // "Pendiente", "EnProceso", "Completado"

        [JsonPropertyName("emailEnviado")]
        public bool EmailEnviado { get; set; }
    }
}
