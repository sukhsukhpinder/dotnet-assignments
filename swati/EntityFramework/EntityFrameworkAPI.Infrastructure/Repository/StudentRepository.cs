using EntityFramework.EntityFrameworkAPI.Infrastructure.Data;
using EntityFrameworkAPI.Core.Entities;
using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Infrastructure.Repository
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(EFDbContext dbContext) : base(dbContext)
        {

        }
    }
}
