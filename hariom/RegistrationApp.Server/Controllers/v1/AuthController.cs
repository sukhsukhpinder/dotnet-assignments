using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RegistarationApp.Core.Models.Auth;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.User;
using RegistrationApp.Core.Services.Interface;
using RegistrationApp.Server.Extensions;

namespace RegistrationApp.Server.Controllers.v1
{
    /// <summary>
    /// Auth controller for auth Apis
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Counstructor
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Generate access token to authorize requests
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [ProducesResponseType<ApiResponse<AuthModel>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Token(LoginModel model)
        {
            return this.Response(StatusCodes.Status200OK, true, Message.LoginSucsess, await _authService.GetAuthToken(model));
        }
        /// <summary>
        /// Generate new token from the refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("refresh_token")]
        [ProducesResponseType<ApiResponse<AuthModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return this.Response(StatusCodes.Status200OK, true, Message.LoginSucsess, await _authService.GenerateNewAccessToken(refreshToken));
        }
    }
}
