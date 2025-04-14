using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Data;
using RestAPI.Models.DTOs;
using RestAPI.Models.Entity;
using RestAPI.Repository;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<User, User, IUsuarioRepository>
    {
        private readonly IUsuarioRepository _UsuarioRepository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected ResponseApi _reponseApi;
        private readonly ApplicationDbContext _context;
        public UsuarioController(IUsuarioRepository UsuarioRepository, IMapper mapper,
            ILogger<UsuarioRepository> logger, ApplicationDbContext context) : base(UsuarioRepository, mapper, logger)
        {
            _UsuarioRepository = UsuarioRepository;
            _reponseApi = new ResponseApi();
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }
    }
}
