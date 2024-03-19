using EnrollHub.Application.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnrollHub.Application.Security
{
    //Use JWt Token if Application WEB API, It is more compatible with WEB API.
    public class JwtTokenManager: IJwtTokenManager
    {
        private readonly IConfiguration _appSettings;
        public JwtTokenManager(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }
        public string GenerateJSONWebTokenWithClaims(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _appSettings["Jwt:Issuer"],
                _appSettings["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public SortedDictionary<string, string> GetAllClaimsFromToken(HttpContext HttpContext)
        {
            SortedDictionary<string, string> claims = new SortedDictionary<string, string>();
            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"].ToString()?.Split(' ').Last();

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                string roleName = jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value;
                claims.Add(Constants.Role, roleName);
            }
            catch (Exception e)
            {
                //claims = null;
            }
            return claims;
        }
    }
}
