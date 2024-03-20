using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Core.UnitOfWork
{
    public interface IDapperUnit : IDisposable
    {
        IDapperStudentRepository DapperStudents { get; }
    }
}
