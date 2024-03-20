
using EntityFrameworkAPI.Core.Entities;
using EntityFrameworkAPI.Core.Repository;

namespace EntityFrameworkAPI.Infrastructure.Repository
{
    public class XMLStudentRepository : XMLRepositoryBase<Student>, IXMLStudentRepository
    {
        public XMLStudentRepository() { }
    }
}
