

using EntityFramework.EntityFrameworkAPI.Core.Common;

namespace EntityFrameworkAPI.Core.Entities
{
    public class Status : EntityBase
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
