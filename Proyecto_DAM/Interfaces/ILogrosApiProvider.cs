using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface ILogrosApiProvider
    {
        Task<IEnumerable<GamificacionDTO>> GetLogros();
        Task<IEnumerable<GamificacionDTO>> GetLogrosRanking();
        Task<GamificacionDTO> GetOneLogro(string id);
        Task PostLogro(GamificacionDTO Logro);
        Task PatchLogro(GamificacionDTO Logro);
        Task<bool> DeleteLogro(string id);
    }
}
