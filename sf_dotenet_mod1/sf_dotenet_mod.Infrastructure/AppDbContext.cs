using Microsoft.EntityFrameworkCore;

namespace sf_dotenet_mod.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
