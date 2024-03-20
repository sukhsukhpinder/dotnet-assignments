
using EntityFramework.EntityFrameworkAPI.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    public class StudentCheckIn : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column]
        public int StudentCheckInId { get; set; }

        [Column]
        public int StudentId { get; set; }
        [Column]
        public DateTime CheckOut { get; set; }
        [Column]
        public DateTime CheckIn { get; set; }



        public virtual Student Student { get; set; }
    }
}
