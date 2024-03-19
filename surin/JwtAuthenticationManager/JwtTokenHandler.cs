using Database;
using JwtAuthenticationManager.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "MySecretKey45e3eruryutytftftjhgy77";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;
        private readonly RegistrationDBContext _context;


        public JwtTokenHandler(RegistrationDBContext context)
        {
            _context = context;

        }

        public async Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.UserName) || string.IsNullOrEmpty(authenticationRequest.Password))
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == authenticationRequest.UserName.ToLower());

            if (user == null) return null;
            else if (!VerifyPasswordHash(authenticationRequest.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            var roleName = await _context.Roles
                .Where(x => x.RoleId == user.RoleId)
                .Select(x => x.RoleName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(roleName))
                return null;

            var tokenExpireTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim("Role", roleName),
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpireTimeStamp,
                SigningCredentials = signingCredentials
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            return new AuthenticationResponse
            {
                UserName = user.UserName,
                ExpiresIn = (int)tokenExpireTimeStamp.Subtract(DateTime.Now).TotalMinutes,
                JwtToken = token
            };
        }

        private static bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(PasswordSalt))
            {
                var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return ComputeHash.SequenceEqual(PasswordHash);
            }
        }

    }
}
