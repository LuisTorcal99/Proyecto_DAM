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
    public class UserApiService : IUserApiProvider
    {
        private readonly IHttpsJsonClientProvider<UsuarioDTO> _httpsJsonClientProvider;

        public UserApiService(IHttpsJsonClientProvider<UsuarioDTO> httpsJsonClientProvider)
        {
            _httpsJsonClientProvider = httpsJsonClientProvider;
        }
        public async Task<IEnumerable<UsuarioDTO>> GetUser()
        {
            return await _httpsJsonClientProvider.GetAsync(Constantes.USUARIO_PATH);
        }
    }
}
