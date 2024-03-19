using EnrollHub.Application.Services.Base;
using EnrollHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnrollHub.Application.ServiceExtensions
{
    public static class Dependencies
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
