namespace RegistarationApp.Core.Models.Auth
{
    public class AuthModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
