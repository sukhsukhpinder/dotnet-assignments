using Registration.API.Dtos;

namespace Registration.API.Services.Interface
{
    public interface IStudentService
    {
        Task<ServiceResponse<bool>> DeleteEnrollmentById(int id);
        Task<ServiceResponse<bool>> DeleteEnrollmentByNumber(string enrollmentNo);
        Task<ServiceResponse<StudentResponse>> EnrollStudent(StudentRequest req);
        Task<ServiceResponse<IEnumerable<StudentResponse>>> GetAll();
        Task<ServiceResponse<StudentResponse>> GetByEnrollmentNo(string enrollmentNo);
        Task<ServiceResponse<StudentResponse>> GetById(int id);
        Task<ServiceResponse<StudentResponse>> UpdateEnrollment(StudentRequest entity, int studentId);
        Task<ServiceResponse<IEnumerable<StatePercentageDto>>> GetStateStudentPercentage();
        Task<ServiceResponse<double>> GetSuccessfulJoinPercentage();
    }


}
