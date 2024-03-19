using Database.Contracts;
using Microsoft.Data.SqlClient;

namespace Database.Implementations.SqlRepository.Common
{
    public class CommonAdoRepository : ICommonContract
    {
        private readonly string _connectionString;

        public CommonAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            List<KeyValuePair<int, string>> courses = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string selectQuery = "SELECT Id, Name FROM Courses WHERE isActive = 1";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int courseId = reader.GetInt32(reader.GetOrdinal("Id"));
                            string courseName = reader.GetString(reader.GetOrdinal("Name"));

                            courses.Add(new KeyValuePair<int, string>(courseId, courseName));
                        }
                    }
                }
            }

            return courses;
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            List<KeyValuePair<int, string>> states = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string selectQuery = "SELECT Id, Name FROM States WHERE isActive = 1";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int stateId = reader.GetInt32(reader.GetOrdinal("Id"));
                            string stateName = reader.GetString(reader.GetOrdinal("Name"));

                            states.Add(new KeyValuePair<int, string>(stateId, stateName));
                        }
                    }
                }
            }

            return states;
        }
    }
}
