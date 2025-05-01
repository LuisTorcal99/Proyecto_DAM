using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;

public class ActualizarPerfilService : IActualizarPerfilProvider
{
    private readonly IUserApiProvider _userApiProvider;
    private readonly IAspNetUserApiProvider _aspNetUserApiService;

    public ActualizarPerfilService(IUserApiProvider userApiProvider, IAspNetUserApiProvider aspNetUserApiService)
    {
        _userApiProvider = userApiProvider;
        _aspNetUserApiService = aspNetUserApiService;
    }

    public async Task<(UsuarioDTO usuario, AppNetUserDto aspNetUser)> ObtenerUsuariosPorId(int userId)
    {
        var allUsers = await _userApiProvider.GetUser();
        var usuario = allUsers.FirstOrDefault(u => u.Id == userId);

        if (usuario == null)
            return (null, null);

        var allAspNetUsers = await _aspNetUserApiService.GetUsers();
        var aspNetUser = allAspNetUsers.FirstOrDefault(a => a.Id == usuario.AspNetUserId);

        return (usuario, aspNetUser);
    }

    public async Task<AppNetUserDto> ObtenerUserAspNetPorId(int userId)
    {
        var allUsers = await _userApiProvider.GetUser();
        var usuario = allUsers.FirstOrDefault(u => u.Id == userId);

        if (usuario == null)
            return null;

        var allAspNetUsers = await _aspNetUserApiService.GetUsers();
        var aspNetUser = allAspNetUsers.FirstOrDefault(a => a.Id == usuario.AspNetUserId);

        return aspNetUser;
    }

    public async Task<bool> ActualizarUsuarios(UsuarioDTO usuario, AppNetUserDto aspNetUser)
    {
        if (usuario == null || aspNetUser == null || usuario.AspNetUserId != aspNetUser.Id)
        {
            Console.WriteLine("Datos inválidos o no coinciden los IDs.");
            return false;
        }

        try
        {
            await _userApiProvider.PatchUser(usuario);
            await _aspNetUserApiService.UpdateUser(aspNetUser);
            Console.WriteLine("Usuarios actualizados correctamente.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar los usuarios: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActualizarAspNetUser(AppNetUserDto aspNetUser)
    {
        if ( aspNetUser == null )
        {
            Console.WriteLine("Datos inválidos o no coinciden los IDs.");
            return false;
        }

        try
        {
            await _aspNetUserApiService.UpdateUser(aspNetUser);
            Console.WriteLine("Usuario actualizado correctamente.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar los usuarios: {ex.Message}");
            return false;
        }
    }
}
