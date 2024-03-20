using EntityFramework.EntityFrameworkAPI.Core.Common;
using EntityFrameworkAPI.Core.Repository;
using System.Xml.Serialization;

namespace EntityFrameworkAPI.Infrastructure.Repository
{
    public class XMLRepositoryBase<T> : IXMLRepositoryBase<T> where T : EntityBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public bool SerializeObjectToXmlFile(T obj, string id, string name)
        {

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(@"Resources/" + name + "_" + id + ".xml"))
            {
                xmlSerializer.Serialize(writer, obj);
            }
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void SerializeListToXmlFile(List<T> obj, string id, string name)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            using (var writer = new StreamWriter(@"Resources/" + name + "_" + id + ".xml"))
            {
                xmlSerializer.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public List<T> DeserializeXmlFileToList(string id)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            var files = System.IO.Directory.GetFiles(@"Resources/", "*" + id + "_*");
            using (var reader = new StreamReader(files[0]))
            {
                return (List<T>)xmlSerializer.Deserialize(reader);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public List<T> DeserializeAllXmlFileToList()
        {
            List<T> list = new List<T>();
            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            var files = System.IO.Directory.GetFiles(@"Resources/");
            //foreach (var file in files)
            //{
            //    using (var reader = new StreamReader(file))
            //    {
            //        var data = (T)xmlSerializer.Deserialize(reader);
            //        list.Add(data);
            //    }
            //}
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public T DeserializeXmlFileToObject(string name)
        {

            var xmlSerializer = new XmlSerializer(typeof(T));
            var files = System.IO.Directory.GetFiles(@"Resources/", name + "_*");
            using (var reader = new StreamReader(files[0]))  //@"Resources/" + id + ".xml"))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public T DeserializeXmlFileToObjectById(string id)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var files = System.IO.Directory.GetFiles(@"Resources/", "*" + id + "*");
            using (var reader = new StreamReader(files[0]))  //@"Resources/" + id + ".xml"))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetStudentId(string name)
        {
            var files = System.IO.Directory.GetFiles(@"Resources/", name + "_*");
            return files[0].Split('_')[1].Split('.')[0].ToString();
        }
    }
}
