using System.ComponentModel.DataAnnotations;

namespace Registration.API.Dtos
{
    public class StudentRequest
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "StateId is required.")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }
        public bool IsActive { get; set; }
    }
}
