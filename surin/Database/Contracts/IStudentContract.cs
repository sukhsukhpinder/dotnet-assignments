using Database.Entities;

namespace Database.Contracts
{
    public interface IStudentContract
    {
        Task<Student> GetById(int id);
        Task<Student> GetByEnrollmentNo(string enrollmentNo);
        Task<IEnumerable<Student>> GetAll();
        Task<Student> EnrollStudent(Student entity);
        Task<Student> UpdateEnrollment(Student entity, int id);
        Task<bool> DeleteEnrollmentByNumber(string enrollmentNo);
        Task<bool> DeleteEnrollmentById(int id);
        Task<IEnumerable<(string? StateName, double Percentage)>> GetStateStudentPercentage();
        // Task<IEnumerable<(string StateName, string Percentage)>> GetStateStudentPercentage();
        Task<double> GetSuccessfulJoinPercentage();
    }
}
