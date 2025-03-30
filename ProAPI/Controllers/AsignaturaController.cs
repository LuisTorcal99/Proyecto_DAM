using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.Asignaturas;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class AsignaturaController : BaseController<AsignaturaEntity, AsignaturaDTO, CreateAsignaturaDTO>
    {
        public AsignaturaController(IAsignaturaRepository AsignaturaRepository,
            IMapper mapper, ILogger<AsignaturaController> logger)
            : base(AsignaturaRepository, mapper, logger)
        {

        }
    }
}
