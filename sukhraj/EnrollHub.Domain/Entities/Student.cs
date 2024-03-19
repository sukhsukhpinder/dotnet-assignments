using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EnrollHub.Domain.Entities
{
    public class Student: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StudentId { get; set; }
        public string EnrollmentNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Column(TypeName = "Date")] // Specify the column type as Date
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public States State { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
