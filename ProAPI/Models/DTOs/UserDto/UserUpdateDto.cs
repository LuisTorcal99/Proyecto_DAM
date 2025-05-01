namespace RestAPI.Models.DTOs.UserDto
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }

}
