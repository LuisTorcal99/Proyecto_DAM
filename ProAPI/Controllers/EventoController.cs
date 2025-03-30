using AutoMapper;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs.Evento;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    public class EventoController : BaseController<EventoEntity, EventoDTO, CreateEventoDTO>
    {
        public EventoController(IEventoRepository EventoRepository,
            IMapper mapper, ILogger<EventoController> logger)
            : base(EventoRepository, mapper, logger)
        {

        }
    }
}
