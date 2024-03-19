using EnrollHub.Application.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EnrollHub.Application.Dtos.Request
{
    public class StudentRequest
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DateOfBirthShouldBeSixteenYear]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Email is Invalid")]
        public string Email { get; set; }
        [IdValidation(0)]
        [DisplayName("State")]
        public int StateId { get; set; }
        [IdValidation(0)]
        [DisplayName("Course")]
        public int CourseId { get; set; }
    }
}
