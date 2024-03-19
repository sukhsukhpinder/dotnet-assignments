using Microsoft.Extensions.Configuration;

namespace sf_dotenet_mod.Infrastructure
{
    public class DataSourceProvider : IDataSourceProvider
    {
        private readonly IConfiguration _configuration;
        public DataSource CurrentDataSource { get; set; }

        public DataSourceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return CurrentDataSource switch
            {
                DataSource.primary => _configuration.GetConnectionString("Primary")!,
                DataSource.secondary => _configuration.GetConnectionString("Secondary")!,
                DataSource.ado => _configuration.GetConnectionString("adoConnection")!,
                DataSource.efCore => _configuration.GetConnectionString("sf_dotenet_modConnection")!,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public interface IDataSourceProvider
    {
        DataSource CurrentDataSource { set; }
        string GetConnectionString();
    }
}
