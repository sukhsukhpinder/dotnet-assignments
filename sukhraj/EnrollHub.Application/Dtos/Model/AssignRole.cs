using System.ComponentModel.DataAnnotations;

namespace EnrollHub.Application.Dtos.Model
{
    public class AssignRole
    {
        [Required(ErrorMessage ="Please Select the Role")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Please Select the User")]
        public string UserId { get; set; }

    }
}
