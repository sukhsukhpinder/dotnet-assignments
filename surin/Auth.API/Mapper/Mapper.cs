using Auth.API.Dto;
using Database.Entities;

namespace Auth.API.Mapper
{
    public static class Mapper
    {
        public static User GetUserEntity(RegistrationRequest request)
        {
            CreatePasswordHash(request.Password, out byte[] PasswordSalt, out byte[] PasswordHash);

            User entity = new User
            {
                UserName = request.UserName,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                Email = request.Email
            };

            return entity;
        }
        public static void CreatePasswordHash(string password, out byte[] PasswordSalt, out byte[] PasswordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                PasswordSalt = hmac.Key; // SALT for password
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
