using Auth.API.Dto;
using JwtAuthenticationManager.Dto;

namespace Auth.API.Services.Interface
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> RegisterUser(RegistrationRequest registrationRequest);
        Task<ServiceResponse<AuthenticationResponse>> AuthenticateUser(AuthenticationRequest authenticationRequest);
    }
}
