using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using SchoolWebAppAPI.Context;
using StudentsManagementApi.Swagger;

namespace SchoolWebAppAPI.Extensions
{
    public static class APIVersioningExtension
    {
        /// <summary>
        /// Extension method for versioning
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection UseAppVersioningHandler(this IServiceCollection services,WebApplicationBuilder builder)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();

                //ApiVersionReader.Combine(new HeaderApiVersionReader("x-api-version"), new QueryStringApiVersionReader("api-version"));
            });
            services
                 .AddApiVersioning()
                .AddVersionedApiExplorer(options =>
                {

                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
            });

            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
            });
            return services;
        }
    }
}
