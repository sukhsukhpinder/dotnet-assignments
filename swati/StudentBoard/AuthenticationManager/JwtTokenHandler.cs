using AuthenticationManager.Data;
using AuthenticationManager.Models;
using EntityFrameworkAPI.Core.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationManager
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        //private readonly List<User> list;
        private readonly string _connectionString;

        public JwtTokenHandler()
        {
            _connectionString = ConfigSettings.ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.UserName) && string.IsNullOrEmpty(authenticationRequest.Password)) { return null; }

            var response = GetStudent(authenticationRequest);
            if (response == null) { return null; }

            var tokenExpiry = DateTime.Now.AddMinutes(Convert.ToUInt32(ConfigSettings.JWT_TOKEN_VALIDITY_MINS));
            var tokenKey = Encoding.ASCII.GetBytes(ConfigSettings.JWT_Security_KEY);

            var claimIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(ClaimTypes.Role, response.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            response.ExpiresIn = (int)tokenExpiry.Subtract(DateTime.Now).TotalSeconds;
            response.JwtToken = token;
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        private dynamic GetStudent(AuthenticationRequest authenticationRequest)
        {
            try
            {
                var hpassword = "";
                var response = new AuthenticationResponse();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT StudentId,Role, PasswordHash FROM Student WHERE StudentName = @StudentName";
                    SqlCommand paramCommand = new SqlCommand(sqlQuery, connection);
                    paramCommand.Parameters.AddWithValue("@StudentName", authenticationRequest.UserName);
                    connection.Open();
                    SqlDataReader paramReader = paramCommand.ExecuteReader();

                    while (paramReader.Read())
                    {
                        hpassword = paramReader["PasswordHash"].ToString();
                        response = new AuthenticationResponse
                        {
                            UserId = paramReader["StudentId"].ToString(),
                            Role = paramReader["Role"].ToString()
                        };
                    }

                    var isVerified = PasswordHasher.VerifyPassword(authenticationRequest.Password, hpassword);

                    if (!isVerified) { return null; }

                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("something went wrong.\n" + e.Message);
                return null;
            }
        }

    }
}
