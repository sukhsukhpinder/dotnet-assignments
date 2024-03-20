using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Core.UnitOfWork
{
    public interface IXMLUnit : IDisposable
    {
        IXMLStudentRepository XMLStudents { get; }
    }
}
