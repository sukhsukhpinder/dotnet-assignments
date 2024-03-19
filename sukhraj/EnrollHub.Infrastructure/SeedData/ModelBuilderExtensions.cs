using EnrollHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnrollHub.Infrastructure.SeedData
{
    public static class ModelBuilderExtensions
    {
        public static void SeedStates(this ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<States>().HasData(
                new States { Id=1, Name = "Andhra Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=2, Name = "Arunachal Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=3, Name = "Punjab", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=4, Name = "Chandigarh", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=5, Name = "Haryana", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=6, Name = "Himachal Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new States { Id=7, Name = "Delhi", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true }
                );
        }

        public static void SeedCourse(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course {Id=1, Name = "BA", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new Course {Id=2, Name = "BCA", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new Course {Id=3, Name = "MBA", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true },
                new Course { Id=4, Name = "BTech", CreatedOn = DateTime.UtcNow, CreatedBy = "Seeder", Active = true }
                );
        }
    }
}
