using System.ComponentModel.DataAnnotations;

namespace sf_dotenet_mod.Application.Dtos.Request
{
    public class UpdateStudentRequest
    {
        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public int StateId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
