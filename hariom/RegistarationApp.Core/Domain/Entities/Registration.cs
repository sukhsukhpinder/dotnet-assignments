using RegistarationApp.Core.Domain.Entities;

namespace RegistartionApp.Core.Domain.Entities
{
    public class Registration : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Constructor
        public Registration()
        {
            // Set default registration date to current date
            RegistrationDate = DateTime.Now;
        }
    }
}
