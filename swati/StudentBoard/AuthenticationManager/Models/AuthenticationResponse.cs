namespace AuthenticationManager.Models
{
    public class AuthenticationResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string JwtToken { get; set; }

        public int ExpiresIn { get; set; }

        public string Role { get; set; }
    }
}
