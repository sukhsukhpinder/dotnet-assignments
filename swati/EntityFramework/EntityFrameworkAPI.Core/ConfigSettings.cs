using Microsoft.Extensions.Configuration;

namespace EntityFrameworkAPI.Common
{
    public static class ConfigSettings1
    {
        public static string ConnectionString { get; }
        public static string CheckInConnectionString { get; }
        public static string JWT_Security_KEY { get; set; }
        public static string JWT_TOKEN_VALIDITY_MINS { get; set; }

        public static string StorageProvider { get; }
        static ConfigSettings1()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            //configurationBuilder.AddJsonFile(path, false);
            ConnectionString = configurationBuilder.Build().GetSection("ConnectionStrings:JWTConnection").Value;
            CheckInConnectionString = configurationBuilder.Build().GetSection("ConnectionStrings:CheckInConnection").Value;
            JWT_Security_KEY = configurationBuilder.Build().GetSection("JWT_Security_KEY").Value;
            JWT_TOKEN_VALIDITY_MINS = configurationBuilder.Build().GetSection("JWT_TOKEN_VALIDITY_MINS").Value;
        }
    }
}
