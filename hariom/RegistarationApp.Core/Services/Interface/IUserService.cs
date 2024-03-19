using RegistarationApp.Core.Models.User;

namespace RegistrationApp.Core.Services.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserModel?> Create(CreateUpdateUserModel model);
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserModel?> Update(string id, CreateUpdateUserModel model);
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(string id);
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfoModel?> GetById(string id);
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserInfoModel?> GetByEmail(string email);
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns></returns>
        Task<List<UserInfoModel>> GetAll();
    }
}
