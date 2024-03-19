using EnrollHub.Domain.Entities;
using EnrollHub.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace EnrollHub.Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EnrollDbContext _dbContext;


        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, EnrollDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }
        public async Task<User> AddUser(User user, string password)
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

        public async Task<bool> AddUserClaim(List<Claim> claims, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false; // User not found
            }

            // Add claims to the user
            var result = await _userManager.AddClaimsAsync(user, claims);

            return result.Succeeded;
        }

        public async Task<bool> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                throw new ArgumentException($"Role {roleName} does not exist", nameof(roleName));
            }

            var allAssignedRoles = await _userManager.GetRolesAsync(user);
            foreach (var assignedRole in allAssignedRoles) //Remove all existing roles
            {
                await _userManager.RemoveFromRoleAsync(user, assignedRole);
            }

            var addRoleResponse = await _userManager.AddToRoleAsync(user, roleName);

            if (!addRoleResponse.Succeeded)
            {
                throw new ApplicationException($"Failed to assign role {roleName} to user {userId}");
            }
            return true;
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var allClaims = new List<Claim>();
            if (user != null)
            {
                // Get user claims
                var userClaims = await _userManager.GetClaimsAsync(user);
                allClaims.AddRange(userClaims);

                // Get user roles
                var userRoles = await _userManager.GetRolesAsync(user);

                // Get role claims
                foreach (var role in userRoles)
                {
                    var roleObject = await _roleManager.FindByNameAsync(role);
                    if (roleObject != null)
                    {
                        var claims = await _roleManager.GetClaimsAsync(roleObject);
                        allClaims.AddRange(claims);
                    }
                }
            }
            return allClaims;
        }

        public async Task<bool> Login(string email, string password, bool rememberMe = false)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var response = await _signInManager.PasswordSignInAsync(email, password, isPersistent: rememberMe, lockoutOnFailure: false);

                if (response.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SignOut()
        {
            await _signInManager.SignOutAsync();
            return true;

        }

        public async Task<List<KeyValuePair<string, string>>> GetAllActiveRoles()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            var keyValuePairs = roles.Select(x => new KeyValuePair<string, string>(x.Id, x.Name)).ToList();
            return keyValuePairs;

        }

        public async Task<List<KeyValuePair<string, string>>> GetAllActiveUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            var keyValuePairs = users.Select(x => new KeyValuePair<string, string>(x.Id, $"{x.FirstName} {x.LastName}")).ToList();
            return keyValuePairs; 
        }

    }
}
