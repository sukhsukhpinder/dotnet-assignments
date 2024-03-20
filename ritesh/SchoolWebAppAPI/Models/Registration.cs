using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebAppAPI.Models
{
    public class Registration : CommonFields
    {
            public int Id { get; set; }
            [MaxLength (50)]
            public string AppName { get; set; }
            [MaxLength(5)]

            public string Passyear { get; set; }
            [MaxLength(10)]
            public string Board { get; set; }
            public string Percentage { get; set; }
            [ForeignKey("State")]
            public int StateId { get; set; }
            public States State { get; set; }
            [ForeignKey("User")]

            public int UserId { get; set; }
            public bool AdmissionTaken { get; set; }
            public User User { get; set; }
    }
}
