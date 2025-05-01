using RestAPI.Data;
using RestAPI.Models.DTOs.UserDto;
using RestAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using RestAPI.Models.Entity;

namespace RestAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ICollection<UserDto>> GetUsers()
        {
            var users = _context.AppUsers.OrderBy(user => user.Email).ToList();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault()
                });
            }

            return userDtos;
        }

        public bool IsUniqueUser(string email)
        {
            return !_context.AppUsers.Any(user => user.Email == email);
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Email.ToLower() == userLoginDto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            //user doesn't exist ?
            if (user == null || !isValid)
            {
                return new UserLoginResponseDto { Token = "", User = null };
            }

            //User does exist
            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
         
            if (secretKey.Length < 32)
            {
                throw new ArgumentException("The secret key must be at least 32 characters long.");
            }
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpirationDays),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto
            {
                Token = tokenHandler.WriteToken(jwtToken),
                User = user
            };
            return userLoginResponseDto;
        }

        public async Task<UserLoginResponseDto?> Register(UserRegistrationDto userRegistrationDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                NormalizedEmail = userRegistrationDto.Email.ToUpper(),
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                //this will run only for first time the roles are created
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("register"));
            }
            await _userManager.AddToRoleAsync(user, "admin");

            //AppUser? newUser = _context.AppUsers.FirstOrDefault(u => u.UserName == userRegistrationDto.UserName);

            var newUser = new User
            {
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password, 
                Rol = userRegistrationDto.Role,
                AspNetUserId = user.Id 
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserLoginResponseDto
            {
                User = user
            };
        }

        public async Task<bool> UpdateAppUserAsync(string appUserId, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(appUserId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
                user.NormalizedEmail = dto.Email.ToUpper();
            }

            // Si se quiere cambiar la contraseña
            if (!string.IsNullOrEmpty(dto.CurrentPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var passwordChanged = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!passwordChanged.Succeeded) return false;
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdatePerfilAsync(string appUserId, UserUpdateDto dto)
        {
            var perfil = _context.Users.FirstOrDefault(u => u.AspNetUserId == appUserId);
            if (perfil == null) return false;

            if (!string.IsNullOrEmpty(dto.Name))
                perfil.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email))
                perfil.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NewPassword))
                perfil.Password = dto.NewPassword;

            _context.Users.Update(perfil);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
