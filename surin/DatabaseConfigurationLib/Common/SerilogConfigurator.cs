using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace DatabaseConfigurationLib.Common
{
    public static class SerilogConfigurator
    {
        public static Logger CreateLogger()
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
