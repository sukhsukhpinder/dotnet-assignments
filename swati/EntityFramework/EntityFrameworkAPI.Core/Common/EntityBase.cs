using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.EntityFrameworkAPI.Core.Common
{
    public abstract class EntityBase
    {
        [Column]
        public string? CreatedBy { get; set; }
        [Column]
        public DateTime? CreatedDate { get; set; }
        [Column]
        public string? LastModifiedBy { get; set; }
        [Column]
        public DateTime? LastModifiedDate { get; set; }
    }
}
