using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.BienestarEstudiantil;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class BienestarEstudiantilController : BaseController<BienestarEstudiantilEntity, BienestarEstudiantilDTO, CrearBienestarEstudiantilDTO>
    {
        public BienestarEstudiantilController(IBienestarEstudiantilRepository BienestarEstudiantilRepository,
            IMapper mapper, ILogger<BienestarEstudiantilController> logger)
            : base(BienestarEstudiantilRepository, mapper, logger)
        {

        }
    }
}
