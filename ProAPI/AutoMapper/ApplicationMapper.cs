using RestAPI.Models.DTOs.UserDto;
using AutoMapper;
using RestAPI.Models.Entity;
using RestAPI.Models.DTOs.Notas;
using RestAPI.Models.DTOs.Asignaturas;
using RestAPI.Models.DTOs.Evento;


namespace RestAPI.AutoMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<NotaEntity, NotasDTO>().ReverseMap();
            CreateMap<CreateNotasDTO, NotaEntity>().ReverseMap();
            CreateMap<AsignaturaEntity, AsignaturaDTO>().ReverseMap();
            CreateMap<CreateAsignaturaDTO, AsignaturaEntity>().ReverseMap();
            CreateMap<EventoEntity, EventoDTO>().ReverseMap();
            CreateMap<CreateEventoDTO, EventoEntity>().ReverseMap();
        }
    }
}
