using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IEventoApiProvider
    {
        Task<IEnumerable<EventoDTO>> GetEvento();
        Task<EventoDTO> GetOneEvento(string id);
        Task PostEvento(EventoDTO Evento);
        Task PatchEvento(EventoDTO Evento);
        Task<bool> DeleteEvento(string id);
    }
}
