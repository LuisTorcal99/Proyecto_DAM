using RestAPI.Models.DTOs.UserDto;
using AutoMapper;
using RestAPI.Models.Entity;
using RestAPI.Models.DTOs.Notas;
using RestAPI.Models.DTOs.Asignaturas;
using RestAPI.Models.DTOs.Evento;
using RestAPI.Models.DTOs.Gamificacion;
using RestAPI.Models.DTOs.BienestarEstudiantil;
using RestAPI.Models.DTOs.RegistroTiempoEstudiado;


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

            CreateMap<GamificacionEntity, GamificacionDTO>().ReverseMap();
            CreateMap<CrearGamificacionDTO, GamificacionEntity>().ReverseMap();

            CreateMap<BienestarEstudiantilEntity, BienestarEstudiantilDTO>().ReverseMap();
            CreateMap<CrearBienestarEstudiantilDTO, BienestarEstudiantilEntity>().ReverseMap();

            CreateMap<RegistroTiempoEstudiadoEntity, RegistroTiempoEstudiadoDTO>().ReverseMap();
            CreateMap<CrearRegistroTiempoEstudiadoDTO, RegistroTiempoEstudiadoEntity>().ReverseMap();

            CreateMap<User, User>().ReverseMap();
        }
    }
}
