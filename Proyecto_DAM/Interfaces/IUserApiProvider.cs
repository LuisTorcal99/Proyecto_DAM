using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IUserApiProvider
    {
        Task<IEnumerable<UsuarioDTO>> GetUser();
        Task<UsuarioDTO> GetOneUser(string id);
        Task PostUser(UsuarioDTO user);
        Task PatchUser(UsuarioDTO user);
        Task<bool> DeleteUser(string id);
    }
}
