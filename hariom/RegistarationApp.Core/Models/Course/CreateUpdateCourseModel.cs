using System.ComponentModel.DataAnnotations;

namespace RegistarationApp.Core.Models.Course
{
    public class CreateUpdateCourseModel
    {
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public int DurationInMonths { get; set; }
        [Required(ErrorMessage = "The field {0} is required", AllowEmptyStrings = false)]
        public double Cost { get; set; }
    }
}
