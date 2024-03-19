using Asp.Versioning;
using Auth.API.Dto;
using Auth.API.Services.Interface;
using JwtAuthenticationManager.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/account")]    
    [ApiVersion("1")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="registrationRequest">The registration request  </param>
        /// <returns>A response indicating the success of the registration </returns>
        [HttpPost("register")]
        [ProducesResponseType<ServiceResponse<IEnumerable<bool>>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ServiceResponse<IEnumerable<string>>>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<bool>>> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var serviceResponse = await _userService.RegisterUser(registrationRequest);

            return StatusCode((int)serviceResponse.Status!, serviceResponse);
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="authenticationRequest">The authentication request </param>
        /// <returns>A response containing the authentication result </returns>
        [HttpPost("Authenticate")]        
        [ProducesResponseType<ServiceResponse<AuthenticationResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<AuthenticationResponse?>>> LogIn([FromBody] AuthenticationRequest authenticationRequest)
        {
            var response = await _userService.AuthenticateUser(authenticationRequest);

            return StatusCode((int)response.Status!, response);
        }

    }
}
