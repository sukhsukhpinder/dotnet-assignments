using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistarationApp.Core.Models.User
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        [PasswordPropertyText]
        public string? Password { get; set; }
    }
}
