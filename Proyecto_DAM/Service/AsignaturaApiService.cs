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
    public class AsignaturaApiService : IAsignaturaApiProvider
    {
        private readonly IHttpsJsonClientProvider<AsignaturaDTO> _httpsJsonClientProvider;

        public AsignaturaApiService(IHttpsJsonClientProvider<AsignaturaDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<AsignaturaDTO>> GetAsignatura()
        {
            return await _httpsJsonClientProvider.GetAsync(Constantes.ASIGNATURA_PATH);
        }

        public async Task<AsignaturaDTO> GetOneAsignatura(string id)
        {
            return await _httpsJsonClientProvider.GetByIdAsync(Constantes.ASIGNATURA_PATH, id);
        }

        public async Task PostAsignatura(AsignaturaDTO Asignatura)
        {
            try
            {
                if (Asignatura == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.ASIGNATURA_PATH, Asignatura);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la Asignatura: {ex.Message}");
            }
        }

        public async Task PatchAsignatura(AsignaturaDTO Asignatura)
        {
            try
            {
                if (Asignatura == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.ASIGNATURA_PATH}/{Asignatura.Id}", Asignatura);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la Asignatura: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsignatura(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.ASIGNATURA_PATH, id);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la Asignatura: {ex.Message}");
                return false;
            }
        }
    }
}
