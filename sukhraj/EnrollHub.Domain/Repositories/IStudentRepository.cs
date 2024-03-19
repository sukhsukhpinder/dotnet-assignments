using EnrollHub.Domain.Entities;

namespace EnrollHub.Domain.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetById(string id);
        Task<Student> GetByEnrollmentNo(string enrollmentNo);
        Task<IEnumerable<Student>> GetAll();
        Task<Student> EnrollStudent(Student entity);
        Task<bool> UpdateEnrollment(Student entity, string id);
        Task<bool> DeleteEnrollmentByNumber(string enrollmentNo);
        Task<bool> DeleteEnrollmentById(string id);
    }
}

