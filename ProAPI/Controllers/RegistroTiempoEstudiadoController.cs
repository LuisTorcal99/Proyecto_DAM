using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.RegistroTiempoEstudiado;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class RegistroTiempoEstudiadoController : BaseController<RegistroTiempoEstudiadoEntity, RegistroTiempoEstudiadoDTO, CrearRegistroTiempoEstudiadoDTO>
    {
        public RegistroTiempoEstudiadoController(IRegistroTiempoEstudiadoRepository RegistroTiempoEstudiadoRepository,
            IMapper mapper, ILogger<RegistroTiempoEstudiadoController> logger)
            : base(RegistroTiempoEstudiadoRepository, mapper, logger)
        {

        }
    }
}
