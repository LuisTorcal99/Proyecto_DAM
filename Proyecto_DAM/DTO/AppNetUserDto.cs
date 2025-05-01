using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proyecto_DAM.DTO
{
    public class AppNetUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
