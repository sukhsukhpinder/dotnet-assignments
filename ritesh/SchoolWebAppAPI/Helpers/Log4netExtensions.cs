using log4net;
using log4net.Config;

namespace SchoolWebAppAPI.Helpers
{
    public static class Log4netExtensions
    {
        /// <summary>
        /// Lognet configuration settings
        /// </summary>
        /// <param name="services"></param>
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
