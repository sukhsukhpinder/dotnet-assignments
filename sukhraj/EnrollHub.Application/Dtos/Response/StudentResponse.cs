using EnrollHub.Application.Dtos.Request;

namespace EnrollHub.Application.Dtos.Response
{
    public class StudentResponse: StudentRequest
    {
     
        public string StudentId { get; set; }
        public string EnrollmentNo { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }

        //Meta Data
        public string StateName { get; set; }
        public string CourseName { get; set; }

    }
}
