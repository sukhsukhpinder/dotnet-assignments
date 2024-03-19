using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Infrastructure.SeedData;

namespace sf_dotenet_mod.Infrastructure
{
    public class EnrollDbContext : IdentityDbContext<Users>
    {
        public EnrollDbContext(DbContextOptions<EnrollDbContext> options) : base(options)
        {

        }

        public DbSet<States> States { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>()
           .HasIndex(s => s.EnrollmentNo)
           .IsUnique();

            // Seed initial data
            modelBuilder.SeedStates();
            modelBuilder.SeedCourse();
        }
    }
}
