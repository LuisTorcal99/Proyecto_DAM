using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;

namespace Proyecto_DAM.Service
{
    public class ActualizarPerfilService : IActualizarPerfilProvider
    {
        private readonly IUserApiProvider _userApiProvider;
        private readonly IAspNetUserApiProvider _aspNetUserApiService;

        public ActualizarPerfilService(IUserApiProvider userApiProvider, IAspNetUserApiProvider aspNetUserApiService)
        {
            _userApiProvider = userApiProvider;
            _aspNetUserApiService = aspNetUserApiService;
        }
        public async Task<bool> Comparar_Y_Actualizar(string userId)
        {
            try
            {
                // Obtener los datos completos del UsuarioDTO usando el id
                UsuarioDTO usuario = await _userApiProvider.GetOneUser(userId);
                if (usuario == null)
                {
                    Console.WriteLine($"No se encontró el usuario con id {userId}");
                    return false;
                }

                // Obtener los datos completos del AppNetUserDto usando el aspNetUserId del usuario
                AppNetUserDto aspNetUser = await _aspNetUserApiService.GetUserById(usuario.AspNetUserId);
                if (aspNetUser == null)
                {
                    Console.WriteLine($"No se encontró el usuario AspNet con id {usuario.AspNetUserId}");
                    return false;
                }

                // Comparar el aspNetUserId de UsuarioDTO con el id de AppNetUserDto
                if (usuario.AspNetUserId == aspNetUser.Id)
                {
                    // Si coinciden, actualizamos ambos usuarios
                    await _userApiProvider.PatchUser(usuario); // Actualizamos el UsuarioDTO
                    await _aspNetUserApiService.UpdateUser(aspNetUser); // Actualizamos el AppNetUserDto

                    Console.WriteLine("Usuarios actualizados correctamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("El aspNetUserId no coincide con el id del usuario.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al comparar o actualizar los usuarios: {ex.Message}");
                return false;
            }
        }
    }
}
