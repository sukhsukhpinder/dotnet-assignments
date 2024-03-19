using Dapper;
using EnrollHub.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EnrollHub.Infrastructure.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly SqlConnection _connection;
        public CommonRepository(IDbConnection dbConnection, SqlConnection connection)
        {
            _dbConnection = dbConnection;
            _connection = connection;
        }

        #region ADO
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            List<KeyValuePair<int, string>> keyValuePairs = new List<KeyValuePair<int, string>>();

            string sqlScript = "SELECT Id, Name FROM Courses WHERE Active=1";


            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using (SqlCommand command = new SqlCommand(sqlScript, _connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        keyValuePairs.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"])));
                    }
                }
            }

            return keyValuePairs;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            List<KeyValuePair<int, string>> keyValuePairs = new List<KeyValuePair<int, string>>();

            string sqlScript = "SELECT Id, Name FROM States WHERE Active=1";

            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using (SqlCommand command = new SqlCommand(sqlScript, _connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        keyValuePairs.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"])));
                    }
                }
            }

            return keyValuePairs;
        }
        #endregion
       
        #region Dapper

        public async Task<DataTable> GetStudentStateWise()
        {
            // Ensure connection is open
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            // Execute query using Dapper and retrieve results into dynamic object list
            var dynamicResult = await _dbConnection.QueryAsync<dynamic>(@"SELECT st.Name as StateName,Count(*) as TotalCount  FROM Students s 
                                                                        INNER JOIN States st ON s.StateId=st.Id 
                                                                        Group By st.Name");
            return ConvertToDataTable(dynamicResult);
        }

        public async Task<DataTable> GetStudentCourseWise()
        {
            // Ensure connection is open
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            // Execute query using Dapper and retrieve results into dynamic object list
            var dynamicResult = await _dbConnection.QueryAsync<dynamic>(@"SELECT c.Name as CourseName, COUNT(*) as TotalCount FROM Students s
                                                                        INNER JOIN Courses c ON s.CourseId = c.Id
                                                                        GROUP BY c.Name    ");
            return ConvertToDataTable(dynamicResult);
        }
        #endregion

        #region PrivateMethod
        private DataTable ConvertToDataTable(IEnumerable<dynamic> dynamicResult)
        {
            DataTable table = new DataTable();

            foreach (var obj in dynamicResult)
            {
                if (table.Columns.Count == 0)
                {
                    foreach (var prop in obj)
                    {
                        table.Columns.Add(prop.Key);
                    }
                }
                DataRow row = table.NewRow();
                foreach (var prop in obj)
                {
                    row[prop.Key] = prop.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        #endregion
    }
}
