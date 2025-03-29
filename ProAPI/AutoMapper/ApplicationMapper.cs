
using RestAPI.Models.DTOs.UserDto;
using AutoMapper;
using RestAPI.Models.Entity;


namespace RestAPI.AutoMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();

        }
    }
}
