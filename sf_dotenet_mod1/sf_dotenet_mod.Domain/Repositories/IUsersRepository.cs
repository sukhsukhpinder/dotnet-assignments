using sf_dotenet_mod.Domain.Entities;

namespace sf_dotenet_mod.Domain.Repositories
{
    /// <summary>
    /// Interface for interacting with user data.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Adds a new user with the provided details and password.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The added user entity.</returns>
        Task<Users> AddUser(Users user, string password);

        /// <summary>
        /// Assigns a role to the user with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>The user entity after assigning the role.</returns>
        Task<Users> AssignRole(string userId, string roleName);
    }
}
