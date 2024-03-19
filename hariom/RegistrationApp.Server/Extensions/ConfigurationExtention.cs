using RegistarationApp.Core.Models.Setting;

namespace RegistrationApp.Server.Extensions
{
    /// <summary>
    /// Configuration Extention to load config
    /// </summary>
    public static class ConfigurationExtention
    {
        /// <summary>
        /// Extention method for handle the configuration
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder UseConfigurationHandler(this WebApplicationBuilder builder)
        {
            RepositorySettings repositorySettings = new();
            builder.Configuration.GetSection("RepositorySettings").Bind(repositorySettings);
            builder.Services.AddSingleton(repositorySettings);

            JwtSetting jwtSetting = new();
            builder.Configuration.GetSection("JwtSetting").Bind(jwtSetting);
            builder.Services.AddSingleton(jwtSetting);

            return builder;
        }
    }
}
