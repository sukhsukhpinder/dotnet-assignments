
using EntityFramework.EntityFrameworkAPI.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    [Table("Student", Schema = "dbo")]
    public class Student : EntityBase
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
        public string PasswordHash { get; set; }
        [Column]
        public DateTime DateOfBirth { get; set; }
        [Column]
        public string Mobile { get; set; }
        [Column]
        public string Gender { get; set; }
        [Column]
        public string Class { get; set; }
        [Column]
        public string FatherName { get; set; }
        [Column]
        public string MotherName { get; set; }
        [Column]
        public DateTime JoinedDate { get; set; }
        [Column]
        public string ImagePath { get; set; }
        [Column]
        public string Role { get; set; }

    }
}
