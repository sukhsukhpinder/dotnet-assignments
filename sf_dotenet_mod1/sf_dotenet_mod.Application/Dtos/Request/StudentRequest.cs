using sf_dotenet_mod.Application.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace sf_dotenet_mod.Application.Dtos.Request
{
    public class StudentRequest
    {
        public string StudentId { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed")]
        public string FirstName { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [MinimumAge(16, ErrorMessage = "You must be at least 16 years old.")]
        [MaximumAge(50, ErrorMessage = "You cannot be older than 50 years old.")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Range(1, int.MaxValue, ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Course is required")]
        public int CourseId { get; set; }
    }
}
