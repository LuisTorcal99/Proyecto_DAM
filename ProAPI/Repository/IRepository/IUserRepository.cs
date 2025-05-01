using RestAPI.Models.DTOs.UserDto;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<ICollection<UserDto>> GetUsers();
        Task<bool> UpdateAppUserAsync(string appUserId, UserUpdateDto dto);
        bool IsUniqueUser(string userName);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserLoginResponseDto> Register(UserRegistrationDto userRegistrationDto);
    }
}
