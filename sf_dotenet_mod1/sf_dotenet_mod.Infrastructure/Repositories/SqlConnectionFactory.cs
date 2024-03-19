using Microsoft.Data.SqlClient;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class SqlConnectionFactory(IDataSourceProvider dataSourceProvider)
    {
        private readonly IDataSourceProvider _dataSourceProvider = dataSourceProvider;

        public SqlConnection CreateConnection()
        {
            _dataSourceProvider.CurrentDataSource = DataSource.ado;
            string connectionString = _dataSourceProvider.GetConnectionString();
            return new SqlConnection(connectionString);
        }
    }
}
