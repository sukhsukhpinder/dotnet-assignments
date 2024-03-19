using Microsoft.IdentityModel.Tokens;
using RegistarationApp.Core.Models.Auth;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.Setting;
using RegistarationApp.Core.Models.User;
using RegistartionApp.Core.Domain.Entities;
using RegistartionApp.Core.Domain.Repositories;
using RegistrationApp.Core.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegistarationApp.Core.Services
{
    /// <summary>
    /// Auth services to generate Token and refresh token
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Store setting for the Jwt auth
        /// </summary>
        private readonly JwtSetting _jwtSetting;

        /// <summary>
        /// Store User repository details
        /// </summary>
        private readonly IRepository<User> _userRepository;
        public AuthService(JwtSetting jwtSetting, IRepository<User> userRepository)
        {
            _jwtSetting = jwtSetting;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Generate New access token from refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AuthModel> GenerateNewAccessToken(string refreshToken)
        {
            // Validate the refresh token
            if (!await ValidateRefreshToken(refreshToken))
            {
                throw new ArgumentException(Message.InvalidRefreshToken);
            }

            // Generate a new access token for the user
            return new AuthModel()
            {
                Token = RegenerateAccessToken(refreshToken),
                RefreshToken = GenerateRefreshToken(GetClaimfromRefreshToken(ClaimTypes.Email, refreshToken), GetClaimfromRefreshToken(ClaimTypes.Role, refreshToken)),
                ExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSetting.ExpiresInMinutes))
            };
        }

        /// <summary>
        /// Generate Access token from the login and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AuthModel> GetAuthToken(LoginModel model)
        {
            var students = await _userRepository.GetAll();
            User? student = students?.FirstOrDefault(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
            if (student != null && student.Password == model.Password)
            {
                return new AuthModel()
                {
                    Token = GenerateAccessToken(student.Email, student.Role),
                    RefreshToken = GenerateRefreshToken(student.Email, student.Role),
                    ExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSetting.ExpiresInMinutes))
                };
            }
            else
            {
                throw new ArgumentException(Message.InvalidEmailPassword);
            }
        }

        /// <summary>
        /// Validate refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key ?? string.Empty)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSetting.Issuer,
                    ValidAudience = _jwtSetting.Audience,
                    ValidateLifetime = false // We are not checking the token's expiration here
                };

                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

                // If the token is successfully validated, it's considered valid
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                // If there's any exception during token validation, the token is considered invalid
                return await Task.FromResult(false);
            }
        }

        #region Private helper methods

        /// <summary>
        /// Generate Access Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private string GenerateAccessToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                //issuer: _jwtSetting.Issuer,
                //audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSetting.ExpiresInMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private string GenerateRefreshToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email), new Claim(ClaimTypes.Role, role) }),
                Expires = DateTime.Now.AddDays(1), // Set the expiration time as desired
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Regenerate Access Token from Refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private string RegenerateAccessToken(string refreshToken)
        {
            // Generate a new access token with the extracted email
            return GenerateAccessToken(GetClaimfromRefreshToken(ClaimTypes.Email, refreshToken), GetClaimfromRefreshToken(ClaimTypes.Role, refreshToken));
        }

        /// <summary>
        /// Get Claim value from Refresh Token
        /// </summary>
        /// <param name="claimTypes"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetClaimfromRefreshToken(string claimTypes, string refreshToken)
        {
            string claimvalue;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key ?? string.Empty);

            try
            {
                // Validate and parse the refresh token
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //issuer: _jwtSetting.Issuer,
                    //audience: _jwtSetting.Audience,
                    ClockSkew = TimeSpan.Zero // This line ensures that the token is not considered expired due to a time difference
                };

                // Extract the claims from the refresh token
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);

                // Retrieve the email claim
                var claim = principal.FindFirst(claimTypes);
                if (claim == null)
                {
                    // The refresh token does not contain an email claim
                    throw new ArgumentException("Refresh token does not contain an email claim");
                }

                // Retrieve the email value
                claimvalue = claim.Value;


            }
            catch (Exception ex)
            {
                // Handle token validation errors
                throw new ArgumentException("Invalid refresh token", ex);
            }

            return claimvalue;
        }

        #endregion
    }
}
