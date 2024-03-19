namespace Registration.API.Dtos
{
    public class StudentResponse : StudentRequest
    {
        public string? FullName { get; set; }
        public string? StudentId { get; set; }
        public string? EnrollmentNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }        
    }
}
