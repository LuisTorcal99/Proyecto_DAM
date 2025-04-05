using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface INotaApiProvider
    {
        Task<IEnumerable<NotaDTO>> GetNota();
        Task<NotaDTO> GetOneNota(string id);
        Task PostNota(NotaDTO Nota);
        Task PatchNota(NotaDTO Nota);
        Task<bool> DeleteNota(string id);
    }
}
