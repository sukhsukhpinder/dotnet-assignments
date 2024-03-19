namespace sf_dotenet_mod.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }

    }
}
