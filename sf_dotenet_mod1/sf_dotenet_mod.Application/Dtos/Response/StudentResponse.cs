using sf_dotenet_mod.Application.Dtos.Request;

namespace sf_dotenet_mod.Application.Dtos.Response
{
    public class StudentResponse : StudentRequest
    {
        public string FullName { get; set; }
        public string EnrollmentNo { get; set; }
        public string StateName { get; set; }
        public string CourseName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}
