using System.ComponentModel.DataAnnotations;

namespace AuthenticationManager.Models
{
    public class AuthenticationRequest //: EntityFrameworkAPI.BaseDto<StudentDto, EntityFrameworkAPI.Core.Entities.Student>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
