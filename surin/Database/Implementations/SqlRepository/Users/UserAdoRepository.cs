using Database.Contracts;
using Database.Entities;
using Microsoft.Data.SqlClient;

namespace Database.Implementations.SqlRepository.Users
{
    //public class UserAdoRepository : IUserContract
    //{
    //    private readonly string _connectionString;

    //    public UserAdoRepository(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }
    //    public async Task<bool> RegisterUser(User user)
    //    {
    //        if (await UserExists(user.UserName))
    //        {
    //            return false;
    //        }

    //        using (SqlConnection connection = new SqlConnection(_connectionString))
    //        {
    //            await connection.OpenAsync();

    //            int defaultRoleId;
    //            using (SqlCommand getDefaultRoleIdCommand = new SqlCommand("SELECT RoleId FROM Roles WHERE RoleName = 'User'", connection))
    //            {
    //                defaultRoleId = (int)await getDefaultRoleIdCommand.ExecuteScalarAsync();
    //            }

    //            string insertQuery = "INSERT INTO Users (UserName, PasswordHash, PasswordSalt, Email, RoleId) " +
    //                                 "VALUES (@UserName, @PasswordHash, @PasswordSalt, @Email, @RoleId)";

    //            using (SqlCommand command = new SqlCommand(insertQuery, connection))
    //            {
    //                command.Parameters.AddWithValue("@UserName", user.UserName);
    //                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
    //                command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
    //                command.Parameters.AddWithValue("@Email", (object)user.Email! ?? DBNull.Value);
    //                command.Parameters.AddWithValue("@RoleId", defaultRoleId);

    //                await command.ExecuteNonQueryAsync();
    //            }
    //        }

    //        return true;
    //    }

    //    public async Task<IEnumerable<Role>> GetRoles(List<string> roleNames)
    //    {
    //        List<Role> roles = new List<Role>();

    //        using (SqlConnection connection = new SqlConnection(_connectionString))
    //        {
    //            await connection.OpenAsync();

    //            string selectQuery = "SELECT * FROM Roles WHERE RoleName IN (@RoleNames)";

    //            using (SqlCommand command = new SqlCommand(selectQuery, connection))
    //            {                    
    //                command.Parameters.AddWithValue("@RoleNames", string.Join(",", roleNames));

    //                using (SqlDataReader reader = await command.ExecuteReaderAsync())
    //                {
    //                    while (await reader.ReadAsync())
    //                    {
    //                        Role role = new Role
    //                        {
    //                            RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
    //                            RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
    //                        };

    //                        roles.Add(role);
    //                    }
    //                }
    //            }
    //        }

    //        return roles;
    //    }

    //    private async Task<bool> UserExists(string userName)
    //    {
    //        using (SqlConnection connection = new SqlConnection(_connectionString))
    //        {
    //            await connection.OpenAsync();

    //            string selectQuery = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";

    //            using (SqlCommand command = new SqlCommand(selectQuery, connection))
    //            {
    //                command.Parameters.AddWithValue("@UserName", userName);

    //                int count = (int)await command.ExecuteScalarAsync();
    //                return count > 0;
    //            }
    //        }
    //    }
    //}

    public class UserAdoRepository : IUserContract
    {
        private readonly string _connectionString;

        public UserAdoRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private async Task<SqlConnection> OpenConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<bool> RegisterUser(User user)
        {
            if (await UserExists(user.UserName!))
            {
                return false;
            }

            using (SqlConnection connection = await OpenConnectionAsync())
            {
                int defaultRoleId;
                using (SqlCommand getDefaultRoleIdCommand = new SqlCommand("SELECT RoleId FROM Roles WHERE RoleName = 'User'", connection))
                {
                    defaultRoleId = (int)await getDefaultRoleIdCommand.ExecuteScalarAsync();
                }

                string insertQuery = "INSERT INTO Users (UserName, PasswordHash, PasswordSalt, Email, RoleId) " +
                                     "VALUES (@UserName, @PasswordHash, @PasswordSalt, @Email, @RoleId)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@Email", (object)user.Email! ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RoleId", defaultRoleId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return true;
        }

        public async Task<IEnumerable<Role>> GetRoles(List<string> roleNames)
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = await OpenConnectionAsync())
            {
                string selectQuery = "SELECT * FROM Roles WHERE RoleName IN (@RoleNames)";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoleNames", string.Join(",", roleNames));

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Role role = new Role
                            {
                                RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                                RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
                            };

                            roles.Add(role);
                        }
                    }
                }
            }

            return roles;
        }

        private async Task<bool> UserExists(string userName)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                string selectQuery = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }
    }
}

