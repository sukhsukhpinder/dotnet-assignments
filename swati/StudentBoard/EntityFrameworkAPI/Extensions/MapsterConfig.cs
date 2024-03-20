
using EntityFrameworkAPI.Services.DTOs;
using Mapster;
using System.Reflection;

namespace EntityFrameworkAPI.Extensions
{
    public static class MapsterConfig
    {
        public static void AddMapster(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
            typeAdapterConfig.Scan(applicationAssembly);
        }
    }
}
