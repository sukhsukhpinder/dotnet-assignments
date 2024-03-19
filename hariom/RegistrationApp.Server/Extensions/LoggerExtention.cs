using Serilog;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Logger Extention to register logging configuration
    /// </summary>
    public static class LoggerExtention
    {
        /// <summary>
        ///  Extention Method for the Logger
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHostBuilder UseLoggerHandler(this IHostBuilder host)
        {
            host.UseSerilog((context, configuration)
                => configuration.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext());
            return host;
        }
    }
}
