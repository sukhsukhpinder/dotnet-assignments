
using EntityFramework.EntityFrameworkAPI.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    [Table("StudentAddress", Schema = "dbo")]
    public class StudentAddress : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentAddressId { get; set; }
        [Column]
        public string HouseNo { get; set; }
        [Column]
        public string City { get; set; }
        [Column]
        public string State { get; set; }
        [Column]
        public string ZipCode { get; set; }
        [Column]
        public string Country { get; set; }
        [Column]
        public int StudentId { get; set; }


        public virtual Student Student { get; set; }
    }
}
