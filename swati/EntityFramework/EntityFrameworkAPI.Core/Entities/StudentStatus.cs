using EntityFramework.EntityFrameworkAPI.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    public class StudentStatus : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentStatusId { get; set; }
        [Column]
        public int StudentId { get; set; }
        [Column]
        public int StatusId { get; set; }


        public virtual Student Student { get; set; }
        public virtual Status Status { get; set; }
    }
}
