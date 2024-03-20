using Microsoft.OpenApi.Models;

namespace SchoolWebAppAPI.Extensions
{
    public static class AddCorsExtension
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            return services;
        }

    }
}
