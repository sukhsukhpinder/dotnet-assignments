using Microsoft.AspNetCore.Identity;

namespace sf_dotenet_mod.Domain.Entities
{
    public class Users : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
