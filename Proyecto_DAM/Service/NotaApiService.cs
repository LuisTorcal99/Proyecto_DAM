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
    public class NotaApiService : INotaApiProvider
    {
        private readonly IHttpsJsonClientProvider<NotaDTO> _httpsJsonClientProvider;

        public NotaApiService(IHttpsJsonClientProvider<NotaDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<NotaDTO>> GetNota()
        {
            return await _httpsJsonClientProvider.GetAsync(Constantes.NOTA_PATH);
        }

        public async Task<NotaDTO> GetOneNota(string id)
        {
            return await _httpsJsonClientProvider.GetByIdAsync(Constantes.NOTA_PATH, id);
        }

        public async Task PostNota(NotaDTO Nota)
        {
            try
            {
                if (Nota == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.NOTA_PATH, Nota);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la Nota: {ex.Message}");
            }
        }

        public async Task PatchNota(NotaDTO Nota)
        {
            try
            {
                if (Nota == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.NOTA_PATH}/{Nota.Id}", Nota);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la Nota: {ex.Message}");
            }
        }

        public async Task<bool> DeleteNota(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.NOTA_PATH, id);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la Nota: {ex.Message}");
                return false;
            }
        }
    }
}
