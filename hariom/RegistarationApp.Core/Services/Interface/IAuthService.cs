using RegistarationApp.Core.Models.Auth;
using RegistarationApp.Core.Models.User;

namespace RegistrationApp.Core.Services.Interface
{
    public interface IAuthService
    {
        /// <summary>
        /// Generate new access token from refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<AuthModel> GenerateNewAccessToken(string refreshToken);
        /// <summary>
        /// Get access token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthModel> GetAuthToken(LoginModel model);
        /// <summary>
        /// validate refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<bool> ValidateRefreshToken(string refreshToken);
    }
}
