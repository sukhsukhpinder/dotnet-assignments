using Serilog;
using Serilog.Core;

namespace sf_dotenet_mod.Configuration
{
    public static class SerilogConfiguration
    {
        public static Logger ConfigureSerilog()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("serilogConfig.json", optional: false, reloadOnChange: true)
                    .Build())
                .CreateLogger();

            return logger;
        }
    }
}
