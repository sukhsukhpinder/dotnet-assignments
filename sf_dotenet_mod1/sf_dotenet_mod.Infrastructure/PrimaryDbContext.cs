using Microsoft.EntityFrameworkCore;

namespace sf_dotenet_mod.Infrastructure
{
    public sealed class PrimaryDbContext : DbContext
    {
        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options) : base(options)
        {
        }
    }
}
