using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistarationApp.Core.Models.User
{
    public class CreateUpdateUserModel
    {
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        [PasswordPropertyText]
        [MinLength(5)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public DateTime DOB { get; set; }
    }
}
