using RegistarationApp.Core.Domain.Entities;

namespace RegistartionApp.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; }
    }
}
