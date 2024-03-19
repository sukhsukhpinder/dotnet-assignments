using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RegistartionApp.Core.Domain.Entities;

namespace RegistrationApp.Core.Domain.Repositories.Database.EntityFrameworkCore
{
    /// <summary>
    /// RegistrationDBContext for EF Core
    /// </summary>
    public class RegistrationDBContext : DbContext
    {
        public RegistrationDBContext() : base()
        {

        }
        public RegistrationDBContext(DbContextOptions<RegistrationDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=RegistrationApp;Trusted_Connection=True;TrustServerCertificate=Yes;");
                base.OnConfiguring(optionsBuilder);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
