using Database.Contracts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Implementations.SqlRepository
{
    public class UserDatacontext : IUserContract
    {
        private readonly RegistrationDBContext _context;

        public UserDatacontext(RegistrationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUser(User user)
        {
            if (await UserExists(user.UserName!))
            {
                return false;
            }
            user.Role = await _context.Roles.SingleAsync(x => x.RoleName == "User");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetRoles(List<string> roleNames)
        {
            return await _context.Roles
                .Where(r => roleNames.Contains(r.RoleName!))
                .ToListAsync();
        }
        private async Task<bool> UserExists(string UserName)
        {
            return await _context.Users.AnyAsync(x => x.UserName!.ToLower() == UserName.ToLower());
        }

    }

}
