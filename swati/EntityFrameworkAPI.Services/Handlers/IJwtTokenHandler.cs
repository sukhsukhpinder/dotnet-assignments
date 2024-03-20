using AuthenticationManager.Models;

namespace EntityFrameworkAPI.Services.Handlers
{
    public interface IJwtTokenHandler
    {
        Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest authenticationRequest);
    }
}
