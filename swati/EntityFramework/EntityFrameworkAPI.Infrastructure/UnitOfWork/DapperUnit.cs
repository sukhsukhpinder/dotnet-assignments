using EntityFrameworkAPI.Core.Repository;
using EntityFrameworkAPI.Core.UnitOfWork;

namespace EntityFrameworkAPI.Infrastructure.UnitOfWork
{
    public class DapperUnit : IDapperUnit
    {
        public IDapperStudentRepository DapperStudents { get; }

        public DapperUnit(IDapperStudentRepository studentRepository)
        {
            DapperStudents = studentRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}
