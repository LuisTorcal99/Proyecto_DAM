using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IAsignaturaApiProvider
    {
        Task<IEnumerable<AsignaturaDTO>> GetAsignatura();
        Task<AsignaturaDTO> GetOneAsignatura(string id);
        Task PostAsignatura(AsignaturaDTO Asignatura);
        Task PatchAsignatura(AsignaturaDTO Asignatura);
        Task<bool> DeleteAsignatura(string id);
    }
}
