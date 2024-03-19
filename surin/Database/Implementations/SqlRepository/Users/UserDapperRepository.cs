using Dapper;
using Database.Contracts;
using Database.Entities;
using System.Data;

namespace Database.Implementations.SqlRepository.Users
{
    public class UserDapperRepository : IUserContract
    {
        private readonly IDbConnection dbConnection;

        public UserDapperRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Role>> GetRoles(List<string> roleNames)
        {
            var sql = "SELECT * FROM Roles WHERE RoleName IN @RoleNames";
            return await dbConnection.QueryAsync<Role>(sql, new { RoleNames = roleNames });
        }

        public async Task<bool> RegisterUser(User user)
        {
            if (await UserExists(user.UserName!))
            {
                return false;
            }

            var roleId = await GetRoleIdByName("User"); 

            var sql = @"INSERT INTO Users (UserName, PasswordHash, PasswordSalt, Email, RoleId)
                    VALUES (@UserName, @PasswordHash, @PasswordSalt, @Email, @RoleId)";

            var affectedRows = await dbConnection.ExecuteAsync(sql, new
            {
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                Email = user.Email,
                RoleId = roleId
            });

            return affectedRows > 0;
        }

        private async Task<bool> UserExists(string UserName)
        {
            var sql = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";
            var count = await dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { UserName = UserName });
            return count > 0;
        }

        private async Task<int> GetRoleIdByName(string roleName)
        {
            var sql = "SELECT RoleId FROM Roles WHERE RoleName = @RoleName";
            return await dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { RoleName = roleName });
        }
    }
}
