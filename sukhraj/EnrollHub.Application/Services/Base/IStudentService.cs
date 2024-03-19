using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Dtos.Response;

namespace EnrollHub.Application.Services.Base
{
    public interface IStudentService
    {
        Task<ServiceResponse<StudentResponse>> GetById(string id);
        Task<ServiceResponse<StudentResponse>> GetByEnrollmentNo(string enrollmentNo);
        Task<ServiceResponse<IEnumerable<StudentResponse>>> GetAll();
        Task<ServiceResponse<StudentResponse>> EnrollStudent(StudentRequest entity, string actionBy);
        Task<ServiceResponse<bool>> UpdateEnrollment(StudentRequest entity, string studentId, string actionBy);
        Task<ServiceResponse<bool>> DeleteEnrollmentByNumber(string enrollmentNo);
        Task<ServiceResponse<bool>> DeleteEnrollmentById(string id);
    }
}
