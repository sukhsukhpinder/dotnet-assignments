using Database.Entities;

namespace Database.Contracts
{
    public interface IUserContract
    {
        Task<bool> RegisterUser(User user);
        Task<IEnumerable<Role>> GetRoles(List<string> roleNames);
    }
}
