using EntityFrameworkAPI.Core.Repository;
using EntityFrameworkAPI.Core.UnitOfWork;

namespace EntityFrameworkAPI.Infrastructure.UnitOfWork
{
    public class XMLUnit : IXMLUnit
    {
        public IXMLStudentRepository XMLStudents { get; }

        public XMLUnit(IXMLStudentRepository studentRepository)
        {
            XMLStudents = studentRepository;
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
