using Proyecto_DAM.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_DAM.Interfaces
{
    public interface IAsignaturaApiProvider
    {
        Task<IEnumerable<AsignaturaDTO>> GetAsignatura();
        Task<AsignaturaDTO> GetOneAsignatura(string id);
        Task PostAsignatura(AsignaturaDTO asignatura);
        Task PatchAsignatura(AsignaturaDTO asignatura);
        Task<bool> DeleteAsignatura(string id);

        Task<TiempoEstudioDTO> GetTiempoEstudio(int idAsignatura);
        Task PatchTiempoEstudio(TiempoEstudioDTO tiempo);
        Task PostTiempoEstudio(TiempoEstudioDTO tiempo);
    }
}
