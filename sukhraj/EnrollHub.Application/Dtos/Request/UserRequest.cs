using System.ComponentModel.DataAnnotations;

namespace EnrollHub.Application.Dtos.Request
{
    public class UserRequest
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")] 
        public string Email { get; set; }
    }
}
