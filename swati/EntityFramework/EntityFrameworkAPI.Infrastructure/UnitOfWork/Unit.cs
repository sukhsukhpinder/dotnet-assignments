
using EntityFramework.EntityFrameworkAPI.Infrastructure.Data;
using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Core.UnitOfWork
{
    public class Unit : IUnit
    {
        private readonly EFDbContext _context;
        public IStudentRepository Students { get; }

        public Unit(EFDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            Students = studentRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
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
                _context.Dispose();
            }
        }
    }
}

