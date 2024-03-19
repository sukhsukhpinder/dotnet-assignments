using EnrollHub.Domain.Entities;
using System.Security.Claims;

namespace EnrollHub.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> AddUser(User user, string password);
        Task<bool> AssignRole(string userId, string roleName);
        Task<bool> Login(string email, string password, bool rememberMe = false);
        Task<bool> SignOut();

        Task<IEnumerable<Claim>> GetUserClaims(string email);
        Task<bool> AddUserClaim(List<Claim> claims, string userId);


        Task<List<KeyValuePair<string, string>>> GetAllActiveUsers();
        Task<List<KeyValuePair<string, string>>> GetAllActiveRoles();

    }
}
