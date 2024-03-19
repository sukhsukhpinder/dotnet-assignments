using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EnrollHub.Application.Security
{
    public interface IJwtTokenManager
    {
        string GenerateJSONWebTokenWithClaims(IEnumerable<Claim> claims);
        SortedDictionary<string, string> GetAllClaimsFromToken(HttpContext HttpContext);
    }
}
