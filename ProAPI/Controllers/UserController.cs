using RestAPI.Models.DTOs;
using RestAPI.Models.DTOs.UserDto;
using RestAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestAPI.Models.Entity;

namespace RestAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _reponseApi;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _reponseApi = new ResponseApi();
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Incorrect Input", message = ModelState });
            }

            if (!_userRepository.IsUniqueUser(userRegistrationDto.Email))
            {
                _reponseApi.StatusCode = HttpStatusCode.BadRequest;
                _reponseApi.IsSuccess = false;
                _reponseApi.ErrorMessages.Add("El correo ya está registrado.");
                return BadRequest(_reponseApi);
            }

            var newUser = await _userRepository.Register(userRegistrationDto);
            if (newUser == null)
            {
                _reponseApi.StatusCode = HttpStatusCode.BadRequest;
                _reponseApi.IsSuccess = false;
                _reponseApi.ErrorMessages.Add("Error al registrar el usuario.");
                return BadRequest(_reponseApi); 
            }

            _reponseApi.StatusCode = HttpStatusCode.OK;
            _reponseApi.IsSuccess = true;
            return Ok(_reponseApi);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var responseLogin = await _userRepository.Login(userLoginDto);

            if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
            {
                _reponseApi.StatusCode = HttpStatusCode.BadRequest;
                _reponseApi.IsSuccess = false;
                _reponseApi.ErrorMessages.Add("Incorrect user and password");
                return BadRequest(_reponseApi);
            }

            _reponseApi.StatusCode = HttpStatusCode.OK;
            _reponseApi.IsSuccess = true;
            _reponseApi.Result = responseLogin;
            return Ok(_reponseApi);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppUser(string id, [FromBody] UserUpdateDto dto)
        {
            var result = await _userRepository.UpdateAppUserAsync(id, dto);
            if (!result) return BadRequest("No se pudo actualizar el usuario (verifica contraseña si aplica).");
            return NoContent();
        }
    }
}
