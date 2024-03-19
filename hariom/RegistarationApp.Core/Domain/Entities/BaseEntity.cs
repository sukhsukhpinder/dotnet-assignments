using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistarationApp.Core.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


        public BaseEntity()
        {
            // Generate a unique registration ID
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
        }
    }
}
