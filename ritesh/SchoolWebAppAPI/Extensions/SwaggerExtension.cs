using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SchoolWebAppAPI.Extensions
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// Swagger extension method to add versioning 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerHandler(this IApplicationBuilder app)
        {

            #region Swagger UI Versioning
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
                c.EnableDeepLinking();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
            });

            #endregion

            return app;
        }
        public static IServiceCollection UseSwaggerGenAuthButton(this IServiceCollection services)
        {

            services.AddSwaggerGen(opt =>
             {
                 opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     In = ParameterLocation.Header,
                     Description = "Please enter token",
                     Name = "Authorization",
                     Type = SecuritySchemeType.Http,
                     BearerFormat = "JWT",
                     Scheme = "bearer"
                 });
                 string xmlFIle = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                 opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFIle));
                 opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                      });
             });
            return services;
        }
    }
}
