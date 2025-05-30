﻿using System;
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
        public const string EXCEL_FILTER = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";
        public const string PDF_FILTER = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        public const string SIMBOLOS_PASSWORD = @"!"";#$%&'()*+,-./:;<=>?@[]^_`{|}~"; 
        public const string PATTERN_CORREO = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string BASE_URL = "http://localhost:7000/api/";
        public const string LOGIN_PATH = "users/login";
        public const string USERS_PATH = "users"; 
        public const string USUARIO_PATH = "Usuario";
        public const string REGISTER_PATH = "users/register";
        public const string ASIGNATURA_PATH = "Asignatura";
        public const string EVENTO_PATH = "Evento";
        public const string NOTA_PATH = "Nota";
        public const string TIEMPO_ESTUDIO_PATH = "RegistroTiempoEstudiado";
        public const string BIENESTAR = "BienestarEstudiantil";
        public const string LOGROS = "Gamificacion";

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
        public const string ERROR_CAMPOSNUMERICO = "El campo de créditos solo acepta números";
        public const string ERROR_NO_DATOS = "No hay datos";

        public const string ROLE_REGISTRER_ADMIN = "admin";

        public const int PUNTOS_TOTALES = 650;
        public const int LOGROS_TOTALES = 16;
        public const string USERNAME = "admin";
        public const string EMAIL = "admin@gmail.com";
        public const string PASSWORD = "ADMINadmin10.";

        public const string USERNAME_GESTOR = "Gestor";
        public const string EMAIL_GESTOR = "gestordeestudio@gmail.com";
        public const string PASSWORD_GESTOR = "ADMINadmin10.";

        public const string RESOURCES_PATH = "/Resources/";
        public const string IMAGES_EXTENSION = ".jpg";
        public const string PATH_IMAGE_NOT_FOUND = "Not_found.png";

        public const string USER_MAIL = "";
        public const string USER_PASS = "";
    }
}
