using AutoMapper;
using RegistarationApp.Core.Models.Course;
using RegistarationApp.Core.Models.User;
using RegistartionApp.Core.Domain.Entities;

namespace RegistrationApp.Server
{
    /// <summary>
    /// Auto Mapper Profile
    /// </summary>
    public class RegistrationAppAutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor for Auto Mapper Profile
        /// </summary>
        public RegistrationAppAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.Birthday));
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.DOB));
            CreateMap<CreateUpdateUserModel, User>()
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.DOB));
            CreateMap<CreateUpdateUserModel, UserModel>();
            CreateMap<UserInfoModel, User>()
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => Convert.ToDateTime(src.DOB)));
            CreateMap<User, UserInfoModel>()
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.Birthday.ToString("dd/MMM/yyyy")));


            CreateMap<Course, CourseModel>();
            CreateMap<CourseModel, Course>();
            CreateMap<CreateUpdateCourseModel, Course>();
        }
    }
}
