using Microsoft.Data.SqlClient;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class AdoCommonRepository(SqlConnectionFactory connection) : ICommonRepository
    {
        private readonly SqlConnectionFactory _connection = connection;

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            List<KeyValuePair<int, string>> keyValuePairs = [];

            string sqlScript = "SELECT Id, Name FROM Courses WHERE Active=1";

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using SqlCommand command = new(sqlScript, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    keyValuePairs.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"])!));
                }
            }

            return keyValuePairs;
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            List<KeyValuePair<int, string>> keyValuePairs = [];

            string sqlScript = "SELECT Id, Name FROM States WHERE Active=1";

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using SqlCommand command = new(sqlScript, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    keyValuePairs.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"])!));
                }
            }

            return keyValuePairs;
        }
    }
}
