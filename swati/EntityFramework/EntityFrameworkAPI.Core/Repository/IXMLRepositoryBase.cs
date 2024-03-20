using EntityFramework.EntityFrameworkAPI.Core.Common;

namespace EntityFrameworkAPI.Core.Repository
{
    public interface IXMLRepositoryBase<T> where T : EntityBase
    {
        bool SerializeObjectToXmlFile(T obj, string Guid, string name);

        void SerializeListToXmlFile(List<T> obj, string id, string name);

        List<T> DeserializeXmlFileToList(string id);

        T DeserializeXmlFileToObject(string id);

        List<T> DeserializeAllXmlFileToList();

        T DeserializeXmlFileToObjectById(string name);

        string GetStudentId(string name);
    }
}
