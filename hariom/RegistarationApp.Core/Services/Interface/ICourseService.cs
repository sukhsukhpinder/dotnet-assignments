using RegistarationApp.Core.Models.Course;

namespace RegistrationApp.Core.Services.Interface
{
    public interface ICourseService
    {
        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<CourseModel?> Create(CreateUpdateCourseModel model);
        /// <summary>
        /// Update Course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<CourseModel?> Update(string id, CreateUpdateCourseModel model);
        /// <summary>
        /// Delete Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(string id);
        /// <summary>
        /// Get Course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CourseModel?> GetById(string id);
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        Task<List<CourseModel>> GetAll();
    }
}
