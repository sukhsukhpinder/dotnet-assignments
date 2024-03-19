using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Dtos.Response;
using EnrollHub.Application.Mappers;
using EnrollHub.Application.Services.Base;
using EnrollHub.Domain.Repositories;

namespace EnrollHub.Application.Services
{
    /// <summary>
    /// Service class providing operations related to student enrollment.
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Deletes enrollment by ID.
        /// </summary>
        /// <param name="id">The ID of the enrollment to delete.</param>
        /// <returns>A service response indicating whether the deletion was successful.</returns>

        public async Task<ServiceResponse<bool>> DeleteEnrollmentById(string id)
        {
            var isDeleted=await _studentRepository.DeleteEnrollmentById(id);
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isDeleted,
                ErrorMessage = isDeleted?"Sucessfully Deleted":"Deletion Failed",
                Result= isDeleted,
            };

            return response;
        }

        /// <summary>
        /// Deletes enrollment by enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student to delete.</param>
        /// <returns>A service response indicating whether the deletion was successful.</returns>

        public async Task<ServiceResponse<bool>> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var isDeleted = await _studentRepository.DeleteEnrollmentByNumber(enrollmentNo);
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isDeleted,
                ErrorMessage = isDeleted ? "Sucessfully Deleted" : "Deletion Failed",
                Result = isDeleted,
            };

            return response;
        }

        /// <summary>
        /// Enrolls a new student.
        /// </summary>
        /// <param name="request">The enrollment request containing student details.</param>
        /// <param name="actionBy">The user initiating the enrollment.</param>
        /// <returns>A service response containing information about the enrolled student.</returns>

        public async Task<ServiceResponse<StudentResponse>> EnrollStudent(StudentRequest request, string actionBy)
        {
            var entity = Mapper.GetStudentEntity(request);
            entity.CreatedBy = actionBy;
            var newEnrollment = await _studentRepository.EnrollStudent(entity);
            var studentResponse= Mapper.GetStudentResponse(newEnrollment);

            var response = new ServiceResponse<StudentResponse>
            {
                IsSuccessful = !string.IsNullOrEmpty(studentResponse.StudentId),
                ErrorMessage = string.IsNullOrEmpty(studentResponse.StudentId) ? Constants.OperationFailed : Constants.OperationSuccessful,
                Result = studentResponse,
            };

            return response;
        }

        /// <summary>
        /// Retrieves all enrolled students.
        /// </summary>
        /// <returns>A service response containing a list of enrolled students.</returns>

        public async Task<ServiceResponse<IEnumerable<StudentResponse>>> GetAll()
        {
            var students= await _studentRepository.GetAll();
            var listOfStudents=students.Select(std=>Mapper.GetStudentResponse(std)).ToList();

            var response = new ServiceResponse<IEnumerable<StudentResponse>>
            {
                IsSuccessful = listOfStudents.Any(),
                ErrorMessage = listOfStudents.Any() ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = listOfStudents,
            };

            return response;
        }
        /// <summary>
        /// Retrieves a student by enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student.</param>
        /// <returns>A service response containing information about the student.</returns>

        public async Task<ServiceResponse<StudentResponse>> GetByEnrollmentNo(string enrollmentNo)
        {
            var student = await _studentRepository.GetByEnrollmentNo(enrollmentNo);
            var studentResponse= Mapper.GetStudentResponse(student);

            var response = new ServiceResponse<StudentResponse>
            {
                IsSuccessful = studentResponse!=null,
                ErrorMessage = studentResponse != null ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = studentResponse,
            };

            return response;
        }
        /// <summary>
        /// Retrieves a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>A service response containing information about the student.</returns>

        public async Task<ServiceResponse<StudentResponse>> GetById(string id)
        {
            var student = await _studentRepository.GetById(id);
            var studentResponse =  Mapper.GetStudentResponse(student);

            var response = new ServiceResponse<StudentResponse>
            {
                IsSuccessful = studentResponse != null,
                ErrorMessage = studentResponse != null ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = studentResponse,
            };

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateEnrollment(StudentRequest request, string studentId, string actionBy)
        {
            var entity=Mapper.GetStudentEntity(request);
            entity.ModifiedBy = actionBy;
            var isUpdated=await  _studentRepository.UpdateEnrollment(entity,studentId);

            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isUpdated,
                ErrorMessage = isUpdated ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = isUpdated,
            };

            return response;
        }
    }
}
