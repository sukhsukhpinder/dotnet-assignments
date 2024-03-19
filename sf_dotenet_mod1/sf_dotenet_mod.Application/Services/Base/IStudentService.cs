using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Application.Dtos.Response;

namespace sf_dotenet_mod.Application.Services.Base
{
    /// <summary>
    /// Interface for a service that provides operations related to students.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Retrieves a student by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the student.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the student information.</returns>
        Task<Response<StudentResponse>> Get(string id);

        /// <summary>
        /// Retrieves a student by its enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the student information.</returns>
        Task<Response<StudentResponse>> GetByEnrollmentNo(string enrollmentNo);

        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with a collection of student information.</returns>
        Task<Response<IEnumerable<StudentResponse>>> GetAll();

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="request">The request containing the information of the student to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the created student information.</returns>
        Task<Response<StudentResponse>> Create(StudentRequest request);

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="request">The request containing the updated information of the student.</param>
        /// <param name="studentId">The unique identifier of the student to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the updated student information.</returns>
        Task<Response<StudentResponse>> Update(StudentRequest request, string studentId);

        /// <summary>
        /// Deletes a student by its enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response indicating whether the deletion was successful.</returns>
        Task<Response<bool>> DeleteEnrollmentByNumber(string enrollmentNo);

        /// <summary>
        /// Deletes a student by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the student to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response indicating whether the deletion was successful.</returns>
        Task<Response<bool>> Delete(string id);

        /// <summary>
        /// Retrieves chart details for students.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the chart details.</returns>
        Task<Response<List<KeyValuePair<string, int>>>> GetChartDetails();
    }
}
