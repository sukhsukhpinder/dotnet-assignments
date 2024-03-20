using EntityFramework.EntityFrameworkAPI.Core.Common;

namespace EntityFrameworkAPI.Core.Entities
{
    public class Subject : EntityBase
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
    }
}
