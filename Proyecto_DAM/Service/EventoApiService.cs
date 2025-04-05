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
    public class EventoApiService : IEventoApiProvider
    {
        private readonly IHttpsJsonClientProvider<EventoDTO> _httpsJsonClientProvider;

        public EventoApiService(IHttpsJsonClientProvider<EventoDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }

        public async Task<IEnumerable<EventoDTO>> GetEvento()
        {
            return await _httpsJsonClientProvider.GetAsync(Constantes.EVENTO_PATH);
        }

        public async Task<EventoDTO> GetOneEvento(string id)
        {
            return await _httpsJsonClientProvider.GetByIdAsync(Constantes.EVENTO_PATH, id);
        }

        public async Task PostEvento(EventoDTO Evento)
        {
            try
            {
                if (Evento == null) return;
                await _httpsJsonClientProvider.PostAsync(Constantes.EVENTO_PATH, Evento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la Evento: {ex.Message}");
            }
        }

        public async Task PatchEvento(EventoDTO Evento)
        {
            try
            {
                if (Evento == null) return;
                await _httpsJsonClientProvider.PatchAsync($"{Constantes.EVENTO_PATH}/{Evento.Id}", Evento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la Evento: {ex.Message}");
            }
        }

        public async Task<bool> DeleteEvento(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return false;
                return await _httpsJsonClientProvider.DeleteAsync(Constantes.EVENTO_PATH, id);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la Evento: {ex.Message}");
                return false;
            }
        }
    }
}
