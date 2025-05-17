using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Proyecto_DAM.Service
{
    public class AsignaturaApiService : IAsignaturaApiProvider
    {
        private readonly IHttpsJsonClientProvider<AsignaturaDTO> _httpsJsonClientProvider;
        private readonly IHttpsJsonClientProvider<TiempoEstudioDTO> _httpsJsonClientTiempo;

        public AsignaturaApiService(IHttpsJsonClientProvider<AsignaturaDTO> httpsJsonClientProvider, IHttpsJsonClientProvider<TiempoEstudioDTO> httpsJsonClientTiempo)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
            _httpsJsonClientTiempo = httpsJsonClientTiempo;
        }

        public async Task<IEnumerable<AsignaturaDTO>> GetAsignatura()
        {
            var todas = await _httpsJsonClientProvider.GetAsync(Constantes.ASIGNATURA_PATH);

            return todas.Where(a => a.IdUsuario == App.Current.Services.GetService<LoginDTO>().Id);
        }

        public async Task<IEnumerable<AsignaturaDTO>> GetAsignaturaIdUserPrueba(int userID)
        {
            var todas = await _httpsJsonClientProvider.GetAsync(Constantes.ASIGNATURA_PATH);

            return todas.Where(a => a.IdUsuario == userID);
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

        public async Task<TiempoEstudioDTO> GetTiempoEstudio(int idAsignatura)
        {
            var todos = await _httpsJsonClientTiempo.GetAsync(Constantes.TIEMPO_ESTUDIO_PATH);
            var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;
            return todos.FirstOrDefault(t => t.AsignaturaID == idAsignatura && t.UsuarioId == idUsuario);
        }

        public async Task PatchTiempoEstudio(TiempoEstudioDTO tiempo)
        {
            await _httpsJsonClientTiempo.PatchAsync($"{Constantes.TIEMPO_ESTUDIO_PATH}/{tiempo.Id}", tiempo);
        }

        public async Task PostTiempoEstudio(TiempoEstudioDTO tiempo)
        {
            try
            {
                if (tiempo == null) return;
                await _httpsJsonClientTiempo.PostAsync(Constantes.TIEMPO_ESTUDIO_PATH, tiempo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el tiempo de estudio: {ex.Message}");
            }
        }


    }
}
