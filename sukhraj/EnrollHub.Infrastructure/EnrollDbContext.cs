using EnrollHub.Domain.Entities;
using EnrollHub.Infrastructure.SeedData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnrollHub.Infrastructure
{
    public class EnrollDbContext:IdentityDbContext<User>
    {
        public EnrollDbContext(DbContextOptions<EnrollDbContext> options):base(options)
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
