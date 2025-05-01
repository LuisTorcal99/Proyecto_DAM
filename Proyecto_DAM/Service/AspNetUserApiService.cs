using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_DAM.Service
{
    public class AspNetUserApiService : IAspNetUserApiProvider
    {
        private readonly IHttpsJsonClientProvider<AppNetUserDto> _httpsJsonClientProvider;

        public AspNetUserApiService(IHttpsJsonClientProvider<AppNetUserDto> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<AppNetUserDto>> GetUsers()
        {
            try
            {
                return await _httpsJsonClientProvider.GetAsync(Constantes.USERS_PATH);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
                return Enumerable.Empty<AppNetUserDto>();
            }
        }

        // Obtener un usuario por su ID
        public async Task<AppNetUserDto> GetUserById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return null;
                return await _httpsJsonClientProvider.GetByIdAsync(Constantes.USERS_PATH, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el usuario con ID {id}: {ex.Message}");
                return null;
            }
        }

        // Actualizar un usuario
        public async Task UpdateUser(AppNetUserDto user)
        {
            try
            {
                if (user == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.USERS_PATH}/{user.Id}", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
            }
        }
    }
}
