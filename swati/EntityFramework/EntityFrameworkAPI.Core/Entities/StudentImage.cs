
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkAPI.Core.Entities
{
    public class StudentImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentImageId { get; set; }
        [Column]
        public string ImageName { get; set; }
        [Column]
        public int StudentId { get; set; }


        public virtual Student Student { get; set; }
    }
}
