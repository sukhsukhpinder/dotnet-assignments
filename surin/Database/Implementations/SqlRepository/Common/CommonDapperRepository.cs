using Dapper;
using Database.Contracts;
using System.Data;

namespace Database.Implementations.SqlRepository.Common
{
    public class CommonDapperRepository : ICommonContract
    {
        private readonly IDbConnection dbConnection;

        public CommonDapperRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            var sql = "SELECT Id, Name FROM Courses WHERE IsActive = 1"; 
            var courses = await dbConnection.QueryAsync<KeyValuePair<int, string>>(sql);
            return courses.ToList();
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            var sql = "SELECT Id, Name FROM States WHERE IsActive = 1"; 
            var states = await dbConnection.QueryAsync<KeyValuePair<int, string>>(sql);
            return states.ToList();
        }
       
        }
    }

