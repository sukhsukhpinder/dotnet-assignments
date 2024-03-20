using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.EntityFramework.Infrastructure.Entities
{
    [Table("Student", Schema = "dbo")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column]
        public int StudentId { get; set; }
        [Column]
        public string StudentName { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string DateOfBirth { get; set; }
        [Column]
        public string Mobile { get; set; }
        [Column]
        public int Gender { get; set; }
        [Column]
        public string Address { get; set; }
        [Column]
        public string Class { get; set; }
        [Column]
        public string FatherName { get; set; }
        [Column]
        public int MotherName { get; set; }
        [Column]
        public int Status { get; set; }

    }
}
