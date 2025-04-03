using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Proyecto_DAM.Utils
{
    public static class Constantes
    {
        public const string BASE_LOCAL_DIRECTORY = "./FILES";
        public const string JSON_FILTER = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public const string SIMBOLOS_PASSWORD = @"!"";#$%&'()*+,-./:;<=>?@[]^_`{|}~"; 
        public const string PATTERN_CORREO = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string BASE_URL = "http://localhost:7000/api/";
        public const string LOGIN_PATH = "users/login";
        public const string USERS_PATH = "users";
        public const string REGISTER_PATH = "users/register";
        public const string ASIGNATURA_PATH = "Asignatura";

        public const string ERROR_TYC = "Debes aceptar los términos y condiciones";
        public const string ERROR_PASSWORDEQUALS = "Contraseñas distintas";
        public const string ERROR_CAMPOSNULL = "Tienes que rellenar todos los campos obligatorios";
        public const string CORREO_NO_VALIDO = "El correo electrónico no es válido";
        public const string LETRA_MAYUSCULA = "La contraseña debe contener al menos una letra mayúscula.";
        public const string LETRA_MINUSCULA = "La contraseña debe contener al menos una letra minúscula.";
        public const string CARACTERES_MASMENOS = "La contraseña debe tener entre 8 y 20 caracteres.";
        public const string LETRA_NUMERO = "La contraseña debe contener al menos un número.";
        public const string LETRA_SIMBOLO = "La contraseña debe contener al menos un simbolo.";
        public const string REGISTRO_EXITOSO = "Usuario registrado con éxito";
        public const string USER_PASSWORD_NOT_GOOD = "Usuario o contraseña incorrectos.";
        public const string MSG_PERFECT = "Perfecto";
        public const string MSG_ERROR = "Ha ocurrido un error";
        public const string ID_ERRONEO = "Error, has introducido un id que no existe";

        public const string ROLE_REGISTRER_ADMIN = "admin";
        public const string ROLE_REGISTRER_USER = "user";
        public const string ROLE_REGISTRER_WPF = "WPF_User";

        public const string USERNAME = "admin";
        public const string EMAIL = "admin@gmail.com";
        public const string PASSWORD = "ADMINadmin10.";

        public const string RESOURCES_PATH = "/Resources/";
        public const string IMAGES_EXTENSION = ".jpg";
        public const string PATH_IMAGE_NOT_FOUND = "Not_found.png";
        public static List<string> PLANETAS_POSIBLES = new List<string>()
        {
            "Planet_1.jpg",
            "Planet_2.jpg"
        };
    }
}
