using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolWebAppAPI.Classes;
using SchoolWebAppAPI.Context;
using SchoolWebAppAPI.Interfaces;
using StudentRegistrationApi.Interfaces;
using StudentsManagementApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SchoolWebAppAPI.Extensions
{
    /// <summary>
    /// To register dependencies
    /// </summary>
    public static class RegisterDependenciesExtension
    {
        /// <summary>
        /// Add Interfaces and classes
        /// </summary>
        /// <param name="services"></param>
        /// <param name="useSQL"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, bool useSQL)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddScoped<IRegistrationData>(provider =>
            {
                var dbContext = provider.GetRequiredService<AppDbContext>();
                var factory = new DataStorageProviderFactory(dbContext);
                return factory.GetProvider(useSQL);
            });
            services.AddScoped<IStudentData>(provider =>
            {
                var dbContext = provider.GetRequiredService<AppDbContext>();
                var factory = new DataStorageProviderFactory(dbContext);
                return factory.GetProviderStudent(useSQL);
            });
            return services;
        }
    }
}
