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
    public class LogrosApiService : ILogrosApiProvider
    {
        private readonly IHttpsJsonClientProvider<GamificacionDTO> _httpsJsonClientProvider;

        public LogrosApiService(IHttpsJsonClientProvider<GamificacionDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<GamificacionDTO>> GetLogros()
        {
            var todas = await _httpsJsonClientProvider.GetAsync(Constantes.LOGROS);

            return todas.Where(a => a.UsuarioId == App.Current.Services.GetService<LoginDTO>().Id);
        }

        public async Task<IEnumerable<GamificacionDTO>> GetLogrosRanking()
        {
            return await _httpsJsonClientProvider.GetAsync(Constantes.LOGROS);
        }

        public async Task<GamificacionDTO> GetOneLogro(string id)
        {
            return await _httpsJsonClientProvider.GetByIdAsync(Constantes.LOGROS, id);
        }

        public async Task PostLogro(GamificacionDTO Logros)
        {
            try
            {
                if (Logros == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.LOGROS, Logros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la Logros: {ex.Message}");
            }
        }

        public async Task PatchLogro(GamificacionDTO Logros)
        {
            try
            {
                if (Logros == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.LOGROS}/{Logros.Id}", Logros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la Logros: {ex.Message}");
            }
        }

        public async Task<bool> DeleteLogro(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.LOGROS, id);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la Logros: {ex.Message}");
                return false;
            }
        }
    }
}
