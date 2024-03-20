

using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Core.UnitOfWork
{
    public interface IUnit : IDisposable
    {
        IStudentRepository Students { get; }
        int Complete();

    }
}
