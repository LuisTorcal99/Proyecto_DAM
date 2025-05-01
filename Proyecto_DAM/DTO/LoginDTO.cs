using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Proyecto_DAM.DTO
{
    public class LoginDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        public string Token { get; set; }
        public int Id { get; set; }

        public LoginDTO(string user, string password)
        {
            Email = user;
            Password = password;
        }

        public LoginDTO()
        {

        }
    }
}
