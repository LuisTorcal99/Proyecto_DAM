﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class UsuarioDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Nombre { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("rol")]
        public string Rol { get; set; }

        [JsonPropertyName("aspNetUserId")]
        public string AspNetUserId { get; set; }

        [JsonPropertyName("aspNetUser")]
        public object AspNetUser { get; set; }

        [JsonPropertyName("asignaturas")]
        public List<string> Asignaturas { get; set; } = new List<string>();

        [JsonPropertyName("eventos")]
        public List<string> Eventos { get; set; } = new List<string>();

        [JsonPropertyName("notas")]
        public List<string> Notas { get; set; } = new List<string>();
    }
}
