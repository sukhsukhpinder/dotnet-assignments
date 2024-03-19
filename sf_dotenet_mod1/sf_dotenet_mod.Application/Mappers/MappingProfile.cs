using AutoMapper;
using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Application.Dtos.Response;
using sf_dotenet_mod.Domain.Entities;

namespace sf_dotenet_mod.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentRequest, Student>();

            CreateMap<Student, StudentResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.State.Id))
            .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.State.Name))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));
        }
    }
}
