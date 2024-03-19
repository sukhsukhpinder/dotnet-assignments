namespace RegistarationApp.Core.Models.Course
{
    public class CourseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DurationInMonths { get; set; }
        public double Cost { get; set; }
    }
}
