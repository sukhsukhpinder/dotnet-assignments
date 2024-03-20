using AuthenticationManager.Models;

namespace AuthenticationManager
{
    public interface IJwtTokenHandler
    {
        Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest authenticationRequest);
    }
}
