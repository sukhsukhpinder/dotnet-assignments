
using EntityFrameworkAPI.Services.Services;
using EntityFrameworkAPI.Services.Services.Interface;

namespace EntityFrameworkAPI.Extensions
{
    public static class ServiceConfig
    {
        public delegate IStudentService ServiceResolver(string provider);

        public static void AddServiceType(this IServiceCollection services)
        {
            services.AddScoped<StudentService>();
            services.AddScoped<XMLService>();
            services.AddScoped<DapperService>();
            services.AddScoped<ServiceResolver>(serviceProvider => provider =>
            {

                switch (provider)
                {
                    case "SqlServer":
                        return serviceProvider.GetService<StudentService>();
                    case "XML":
                        return serviceProvider.GetService<XMLService>();
                    case "Dapper":
                        return serviceProvider.GetService<DapperService>();
                    default:
                        return null;
                }
            });
        }
    }
}
