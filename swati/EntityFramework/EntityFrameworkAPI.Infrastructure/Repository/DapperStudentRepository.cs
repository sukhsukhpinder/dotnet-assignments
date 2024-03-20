using EntityFrameworkAPI.Core.Entities;
using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Infrastructure.Repository
{
    public class DapperStudentRepository : DapperRepositoryBase<Student>, IDapperStudentRepository
    {
        public DapperStudentRepository()
        {

        }
    }
}
