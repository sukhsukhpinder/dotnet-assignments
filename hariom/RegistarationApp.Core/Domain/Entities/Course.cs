using RegistarationApp.Core.Domain.Entities;

namespace RegistartionApp.Core.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DurationInMonths { get; set; }
        public double Cost { get; set; }

    }
}
