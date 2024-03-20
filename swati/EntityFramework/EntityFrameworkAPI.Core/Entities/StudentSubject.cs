
using EntityFramework.EntityFrameworkAPI.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    public class StudentSubject : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentSubjectId { get; set; }
        [Column]
        public int StudentId { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int SubjectId { get; set; }



        public virtual Student Student { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
