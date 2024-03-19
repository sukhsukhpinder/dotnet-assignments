using Microsoft.OpenApi.Models;
using System.Reflection;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Swagger Service Extensions
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Use Swagger Service Handler
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseSwaggerServiceHandler(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer AbCdEf123456\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {

                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                //show XMl documentation for the API               

                string xmlFIle = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                if (File.Exists(Path.Combine(AppContext.BaseDirectory, xmlFIle)))
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFIle));
            });
            return services;
        }
    }
}
