using Microsoft.Extensions.Configuration;

namespace AuthenticationManager.Data
{
    public static class ConfigSettings
    {
        public static string ConnectionString { get; }
        public static string CheckInConnectionString { get; }
        public static string JWT_Security_KEY { get; set; }
        public static string JWT_TOKEN_VALIDITY_MINS { get; set; }

        public static string StorageProvider { get; }
        static ConfigSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            StorageProvider = configurationBuilder.Build().GetSection("StorageProvider").Value;
            ConnectionString = configurationBuilder.Build().GetSection("ConnectionStrings:JWTConnection").Value;
            CheckInConnectionString = configurationBuilder.Build().GetSection("ConnectionStrings:CheckInConnection").Value;
            JWT_Security_KEY = configurationBuilder.Build().GetSection("JWT_Security_KEY").Value;
            JWT_TOKEN_VALIDITY_MINS = configurationBuilder.Build().GetSection("JWT_TOKEN_VALIDITY_MINS").Value;
        }
    }
}
