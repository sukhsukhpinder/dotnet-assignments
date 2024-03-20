
using EntityFramework.EntityFrameworkAPI.Core.Common;
using EntityFrameworkAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.EntityFrameworkAPI.Infrastructure.Data
{
    public class EFDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public EFDbContext(DbContextOptions<EFDbContext> dbContext) : base(dbContext)
        {
            try
            {
                //var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                //if (databaseCreator != null)
                //{
                //    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                //    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Status> Statuse { get; set; }
        public DbSet<StudentAddress> StudentAddress { get; set; }
        public DbSet<StudentStatus> StudentStatus { get; set; }
        public DbSet<StudentSubject> StudentSubject { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "ss";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "ss";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
