using EntityFramework.EntityFrameworkAPI.Core.Common;
using EntityFrameworkAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkAPI.Infrastructure.Data
{
    public class EFCodeContext : DbContext
    {
        public EFCodeContext(DbContextOptions<EFCodeContext> dbContext) : base(dbContext)
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

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StudentAddress> StudentAddresses { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Student>().HasKey(m => m.StudentId);

        //    //modelBuilder.Entity<BlogHeader>()
        //    //.HasOne(e => e.Blog)
        //    //    .WithOne(e => e.Header)
        //    //.HasForeignKey<BlogHeader>(e => e.BlogId)
        //    //.IsRequired();

        //    modelBuilder.Entity<Student>()
        //    .HasOne(s => s.Address)
        //    .WithOne(ad => ad.Student)
        //    .HasForeignKey<StudentAddress>(ad => new { AddressStudentId = ad.StudentAddressId });

        //    modelBuilder.Entity<Student>()
        //    .HasOne<StudentStatus>(s => s.Status)
        //    .WithOne(ad => ad.Student)
        //    .HasForeignKey<StudentStatus>(ad => ad.StudentStatusId);

        //    modelBuilder.Entity<Student>().HasOne(v => v.Subject).WithMany().HasForeignKey(v => v.Subject.StudentId);

        //    base.OnModelCreating(modelBuilder);
        //}

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
