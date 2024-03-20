using Microsoft.EntityFrameworkCore;
using SchoolWebAppAPI.Models;

namespace SchoolWebAppAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Registration> Registration { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<States>().ToTable("states");
            builder.Entity<Registration>().ToTable("registration");
        }
    }
}
