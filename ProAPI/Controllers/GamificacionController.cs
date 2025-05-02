using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.Asignaturas;
using RestAPI.Models.DTOs.Gamificacion;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class GamificacionController : BaseController<GamificacionEntity, GamificacionDTO, CrearGamificacionDTO>
    {
        public GamificacionController(IGamificacionRepository GamificacionRepository,
            IMapper mapper, ILogger<GamificacionController> logger)
            : base(GamificacionRepository, mapper, logger)
        {

        }
    }
}
