using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IEventoNotificacionProvider
    {
        Task VerificarYEnviarCorreos(List<EventoDTO> eventos, List<NotaDTO> notas, string idUsuario);
    }
}
