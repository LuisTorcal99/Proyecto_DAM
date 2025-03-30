using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.Notas;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class NotaController : BaseController<NotaEntity, NotasDTO, CreateNotasDTO>
    {
        public NotaController(INotaRepository NotaRepository,
            IMapper mapper, ILogger<NotaController> logger)
            : base(NotaRepository, mapper, logger)
        {

        }
    }
}
