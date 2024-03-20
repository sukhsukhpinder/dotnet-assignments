using Serilog;

namespace EntityFrameworkAPI.Extensions
{
    public static class SerilogConfig
    {
        public static void AddSeriLogger(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            var _logger = new LoggerConfiguration()
            .ReadFrom.Configuration(webApplicationBuilder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

            services.AddSerilog(_logger);
        }
    }
}
