using Database.Entities;
using Enrollment.Database.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class RegistrationDBContext : DbContext
    {
        public RegistrationDBContext(DbContextOptions<RegistrationDBContext> options)
       : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
             .HasIndex(s => s.EnrollmentNo)
             .IsUnique();

            modelBuilder.Entity<Student>()
               .HasOne(s => s.Course)
               .WithMany(c => c.Students)
               .HasForeignKey(s => s.CourseId)
               .OnDelete(DeleteBehavior.Cascade);



            // Seed initial data
            modelBuilder.SeedStates();
            modelBuilder.SeedCourse();
            modelBuilder.SeedRoles();
        }
    }
}
