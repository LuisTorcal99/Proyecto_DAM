using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.Service
{
    public class BienestarApiService : IBienestarApiProvider
    {
        private readonly IHttpsJsonClientProvider<BienestarDTO> _httpsJsonClientProvider;

        public BienestarApiService(IHttpsJsonClientProvider<BienestarDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<BienestarDTO>> GetBienestar()
        {
            var todas = await _httpsJsonClientProvider.GetAsync(Constantes.BIENESTAR);

            return todas.Where(a => a.UsuarioId == App.Current.Services.GetService<LoginDTO>().Id);
        }

        public async Task<BienestarDTO> GetOneBienestar(string id)
        {
            return await _httpsJsonClientProvider.GetByIdAsync(Constantes.BIENESTAR, id);
        }

        public async Task PostBienestar(BienestarDTO Bienestar)
        {
            try
            {
                if (Bienestar == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.BIENESTAR, Bienestar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la Bienestar: {ex.Message}");
            }
        }

        public async Task PatchBienestar(BienestarDTO Bienestar)
        {
            try
            {
                if (Bienestar == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.BIENESTAR}/{Bienestar.Id}", Bienestar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la Bienestar: {ex.Message}");
            }
        }

        public async Task<bool> DeleteBienestar(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.BIENESTAR, id);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la Bienestar: {ex.Message}");
                return false;
            }
        }
    }
}
