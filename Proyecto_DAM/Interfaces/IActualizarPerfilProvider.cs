using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IActualizarPerfilProvider
    {
        Task<(UsuarioDTO usuario, AppNetUserDto aspNetUser)> ObtenerUsuariosPorId(int userId);
        Task<bool> ActualizarUsuarios(UsuarioDTO usuario, AppNetUserDto aspNetUser);
        Task<AppNetUserDto> ObtenerUserAspNetPorId(int userId);
        Task<bool> ActualizarAspNetUser(AppNetUserDto aspNetUser);
    }
}
