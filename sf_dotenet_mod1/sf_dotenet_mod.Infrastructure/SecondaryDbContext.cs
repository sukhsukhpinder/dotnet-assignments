using Microsoft.EntityFrameworkCore;

namespace sf_dotenet_mod.Infrastructure
{
    public class SecondaryDbContext : DbContext
    {
        public SecondaryDbContext(DbContextOptions<SecondaryDbContext> options) : base(options)
        {
        }
    }
}
