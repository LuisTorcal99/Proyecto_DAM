using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.Service
{
    public class UserApiService : IUserApiProvider
    {
        private readonly IHttpsJsonClientProvider<UsuarioDTO> _httpsJsonClientProvider;

        public UserApiService(IHttpsJsonClientProvider<UsuarioDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUser()
        {
            try
            {
                return await _httpsJsonClientProvider.GetAsync(Constantes.USUARIO_PATH);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
                return Enumerable.Empty<UsuarioDTO>();
            }
        }

        public async Task<UsuarioDTO> GetOneUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return null;
                return await _httpsJsonClientProvider.GetByIdAsync(Constantes.USUARIO_PATH, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el usuario: {ex.Message}");
                return null;
            }
        }

        public async Task PostUser(UsuarioDTO user)
        {
            try
            {
                if (user == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.USUARIO_PATH, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el usuario: {ex.Message}");
            }
        }

        public async Task PatchUser(UsuarioDTO user)
        {
            try
            {
                if (user == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.USUARIO_PATH}/{user.Id}", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.USUARIO_PATH, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }
    }
}
