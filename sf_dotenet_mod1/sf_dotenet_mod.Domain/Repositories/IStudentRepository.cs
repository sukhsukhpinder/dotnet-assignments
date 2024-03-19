using sf_dotenet_mod.Domain.Entities;

namespace sf_dotenet_mod.Domain.Repositories
{
    /// <summary>
    /// Interface for interacting with student data.
    /// </summary>
    public interface IStudentRepository
    {
        /// <summary>
        /// Retrieves a student entity by its unique identifier.
        /// </summary>
        Task<Student> Get(string id);

        /// <summary>
        /// Retrieves a student entity by its enrollment number.
        /// </summary>
        Task<Student> GetByEnrollmentNo(string enrollmentNo);

        /// <summary>
        /// Retrieves all student entities.
        /// </summary>
        Task<IEnumerable<Student>> GetAll();

        /// <summary>
        /// Creates a new student entity.
        /// </summary>
        Task<Student> Create(Student entity);

        /// <summary>
        /// Updates an existing student entity.
        /// </summary>
        Task<Student> Update(Student entity, string id);

        /// <summary>
        /// Deletes a student entity by its enrollment number.
        /// </summary>
        Task<bool> DeleteEnrollmentByNumber(string enrollmentNo);

        /// <summary>
        /// Deletes a student entity by its unique identifier.
        /// </summary>
        Task<bool> Delete(string id);

        /// <summary>
        /// Retrieves chart details for data visualization.
        /// </summary>
        Task<List<KeyValuePair<string, int>>> GetChartDetails();
    }
}
