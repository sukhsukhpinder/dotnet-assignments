using System.ComponentModel.DataAnnotations;

namespace SchoolWebAppAPI.Models
{
    public class User: CommonFields
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Username { get; set; }
        public int? Age { get; set; }
        [MaxLength(12)]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
