using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IBienestarApiProvider
    {
        Task<IEnumerable<BienestarDTO>> GetBienestar();
        Task<BienestarDTO> GetOneBienestar(string id);
        Task PostBienestar(BienestarDTO Bienestar);
        Task PatchBienestar(BienestarDTO Bienestar);
        Task<bool> DeleteBienestar(string id);
    }
}
