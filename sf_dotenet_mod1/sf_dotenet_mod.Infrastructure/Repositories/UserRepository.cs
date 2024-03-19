using Microsoft.AspNetCore.Identity;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EnrollDbContext _dbContext;

        public UserRepository(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EnrollDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }
        public async Task<Users> AddUser(Users user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var response = await _userManager.CreateAsync(user, password);

                    if (response.Succeeded)
                    {
                        var roleResponse = await _userManager.AddToRoleAsync(user, "User"); /*default role*/

                        if (roleResponse.Succeeded)
                        {
                            await transaction.CommitAsync();
                            return user;
                        }
                    }

                    // If any step fails, rollback the transaction
                    await transaction.RollbackAsync();
                    user.Id = string.Empty;
                    return user;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error occurred while adding user", ex);
                }
            }
        }

        public async Task<Users> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
                }

                // Check if the role exists

                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    throw new ArgumentException($"Role {roleName} does not exist", nameof(roleName));
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Failed to assign role {roleName} to user {userId}");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while assigning role to user", ex);
            }
        }

    }
}
