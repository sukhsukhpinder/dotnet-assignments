using Database.Contracts;
using Database.Entities;
using Newtonsoft.Json;

namespace Database.Implementations.FileRepository
{
    public class FileUserRepository : IUserContract
    {
        //private readonly string filePath = Environment.CurrentDirectory + "\\FileUserDb.txt";
        string filePath = @"D:\Project\RegistrationSystem\RegistrationSystem\Database\FileDB";
        //public async Task<bool> RegisterUser(User user)
        //{

        //    if (await UserExists(user.UserName))
        //    {
        //        return false;
        //    }

        //    //List<Role> roles = await GetRolesFromFile(Environment.CurrentDirectory + "\\Roles.txt", new List<string> { "User" });
        //    List<Role> roles = await GetRolesFromFile(filePath + "\\Roles.txt", new List<string> { "User" });
        //    user.Role = roles.FirstOrDefault();

        //    var users = await ReadUsersFromFile();
        //    users.Add(user);

        //    await WriteUsersToFile(users);

        //    return true;

        //}
        public async Task<bool> RegisterUser(User user)
        {
            if (await UserExists(user.UserName!))
            {
                return false;
            }

            List<Role> roles = await GetRolesFromFile(filePath + "\\Roles.txt", new List<string> { "User" });
            user.Role = roles.FirstOrDefault();

            var users = await ReadUsersFromFile();

            user.UserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;

            users.Add(user);

            await WriteUsersToFile(users);

            return true;
        }
        public async Task<IEnumerable<Role>> GetRoles(List<string> roleNames)
        {

            return await GetRolesFromFile(filePath + "\\Roles.txt", roleNames);
        }

        private async Task<List<User>> ReadUsersFromFile()
        {

            if (File.Exists(filePath + "\\FileUserDb.txt"))
            {
                var json = await File.ReadAllTextAsync(filePath + "\\FileUserDb.txt");
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }

            return new List<User>();
        }

        private async Task WriteUsersToFile(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            await File.WriteAllTextAsync(filePath + "\\FileUserDb.txt", json);
        }

        private async Task<List<Role>> GetRolesFromFile(string rolesFilePath, List<string> roleNames)
        {
            List<Role> roles = new List<Role>();


            if (!File.Exists(rolesFilePath))
            {
                CreateDefaultRolesFile(rolesFilePath);
            }

            using (StreamReader reader = new StreamReader(rolesFilePath))
            {
                string json = await reader.ReadToEndAsync();
                roles = JsonConvert.DeserializeObject<List<Role>>(json) ?? new List<Role>();
                roles = roles.Where(r => roleNames.Contains(r.RoleName!)).ToList();
            }

            return roles;
        }

        private void CreateDefaultRolesFile(string rolesFilePath)
        {
            List<Role> defaultRoles = new List<Role>
              {
                 new Role { RoleName = "Admin" },
                 new Role { RoleName = "User" }
              };

            string json = JsonConvert.SerializeObject(defaultRoles, Formatting.Indented);
            File.WriteAllText(rolesFilePath, json);

        }
        private async Task<bool> UserExists(string userName)
        {
            var users = await ReadUsersFromFile();
            return users.Any(u => u.UserName!.ToLower() == userName.ToLower());
        }
    }
}
