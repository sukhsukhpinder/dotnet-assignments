using Database.Contracts;
using Registration.API.Constants;
using Registration.API.Dtos;
using Registration.API.Services.Interface;
using System.Net;

namespace Registration.API.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentContract studentData;

        public StudentService(IStudentContract studentData)
        {
            this.studentData = studentData;
        }        

        public async Task<ServiceResponse<bool>> DeleteEnrollmentById(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var response = await studentData.DeleteEnrollmentById(id);
            SetResponseProperties(serviceResponse, response, HttpStatusCode.NoContent,
                HttpStatusCode.NotFound,ResponseConstants.Failed);
            return serviceResponse;
        }
        public async Task<ServiceResponse<bool>> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var response = await studentData.DeleteEnrollmentByNumber(enrollmentNo);
            SetResponseProperties(serviceResponse, response, HttpStatusCode.NoContent, 
                HttpStatusCode.NotFound, ResponseConstants.Failed);

            return serviceResponse;
        }

        public async Task<ServiceResponse<StudentResponse>> EnrollStudent(StudentRequest req)
        {
            var serviceResponse = new ServiceResponse<StudentResponse>();
            var entity = Mapper.Mapper.GetStudentEntity(req);
            var newEnrollment = await studentData.EnrollStudent(entity);

            SetResponseProperties(serviceResponse!, newEnrollment != null ? Mapper.Mapper.GetStudentResponse(newEnrollment)
                : null, HttpStatusCode.Created, HttpStatusCode.InternalServerError, ResponseConstants.FailedEnrollment);

            return serviceResponse;
        }


        public async Task<ServiceResponse<IEnumerable<StudentResponse>>> GetAll()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<StudentResponse>>();
            var students = await studentData.GetAll();

            SetResponseProperties(serviceResponse!, students?.Select(Mapper.Mapper.GetStudentResponse),
                HttpStatusCode.OK, HttpStatusCode.InternalServerError, ResponseConstants.FailedStudentsFetch);

            return serviceResponse;
        }



        public async Task<ServiceResponse<StudentResponse>> GetByEnrollmentNo(string enrollmentNo)
        {
            var serviceResponse = new ServiceResponse<StudentResponse>();
            var student = await studentData.GetByEnrollmentNo(enrollmentNo);

            SetResponseProperties(serviceResponse!, student != null ? Mapper.Mapper.GetStudentResponse(student)
                : null, HttpStatusCode.OK, HttpStatusCode.NotFound, ResponseConstants.NotFound);

            return serviceResponse;
        }


        public async Task<ServiceResponse<StudentResponse>> GetById(int id)
        {
            var serviceResponse = new ServiceResponse<StudentResponse>();
            var student = await studentData.GetById(id);
            SetResponseProperties(serviceResponse, Mapper.Mapper.GetStudentResponse(student),
                HttpStatusCode.OK, HttpStatusCode.NotFound, ResponseConstants.NotFound);
            return serviceResponse;
        }

        public async Task<ServiceResponse<StudentResponse>> UpdateEnrollment(StudentRequest entity, int studentId)
        {
            var serviceResponse = new ServiceResponse<StudentResponse>();
            var updatedStudent = await studentData.UpdateEnrollment(Mapper.Mapper.GetStudentEntity(entity), studentId);

            SetResponseProperties(serviceResponse!, updatedStudent != null ? Mapper.Mapper.GetStudentResponse(updatedStudent)
                : null, HttpStatusCode.OK, HttpStatusCode.NotFound, ResponseConstants.NotFound);

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<StatePercentageDto>>> GetStateStudentPercentage()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<StatePercentageDto>>();
            var statePercentages = await studentData.GetStateStudentPercentage();

            SetResponseProperties(serviceResponse!, statePercentages?.Select(result => new StatePercentageDto
            {
                StateName = result.StateName,
                Percentage = result.Percentage
            }), HttpStatusCode.OK, HttpStatusCode.NotFound, ResponseConstants.NoStatePercentagesFoundError);

            return serviceResponse;
        }

        public async Task<ServiceResponse<double>> GetSuccessfulJoinPercentage()
        {
            var serviceResponse = new ServiceResponse<double>();
            double percentage = await studentData.GetSuccessfulJoinPercentage();

            SetResponseProperties(serviceResponse, percentage >= 0 ? percentage 
                : default(double), HttpStatusCode.OK, HttpStatusCode.InternalServerError,
                ResponseConstants.FetchingJoinPercentageError);

            return serviceResponse;
        }

        private void SetResponseProperties<T>(ServiceResponse<T> serviceResponse, T result, HttpStatusCode successStatusCode, HttpStatusCode errorStatusCode, string? errorDetails = null)
        {
            serviceResponse.IsSuccessful = result != null && !EqualityComparer<T>.Default.Equals(result, default(T));
            serviceResponse.Result = result;
            serviceResponse.Status = serviceResponse.IsSuccessful ? successStatusCode : errorStatusCode;
            serviceResponse.ErrorDetails = serviceResponse.IsSuccessful ? null : errorDetails;
        }

        //public async Task<ServiceResponse<bool>> DeleteEnrollmentById(int id)
        //{
        //    var serviceResponse = new ServiceResponse<bool>();

        //    var response = await studentData.DeleteEnrollmentById(id);
        //    if (response)
        //    {
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.NoContent;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //        serviceResponse.ErrorDetails = ResponseConstants.NotFound;
        //    }
        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<bool>> DeleteEnrollmentByNumber(string enrollmentNo)
        //{
        //    var serviceResponse = new ServiceResponse<bool>();

        //    var response = await studentData.DeleteEnrollmentByNumber(enrollmentNo);

        //    if (response)
        //    {
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.NoContent;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //        serviceResponse.ErrorDetails = ResponseConstants.NotFound;
        //    }
        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<StudentResponse>> EnrollStudent(StudentRequest req)
        //{
        //    var serviceResponse = new ServiceResponse<StudentResponse>();


        //    var entity = Mapper.Mapper.GetStudentEntity(req);
        //    var newEnrollment = await studentData.EnrollStudent(entity);

        //    if (newEnrollment != null)
        //    {
        //        var response = Mapper.Mapper.GetStudentResponse(newEnrollment);
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.Created;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.Status = HttpStatusCode.InternalServerError;
        //        serviceResponse.Result = null;
        //    }

        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<IEnumerable<StudentResponse>>> GetAll()
        //{
        //    var serviceResponse = new ServiceResponse<IEnumerable<StudentResponse>>();
        //    var students = await studentData.GetAll();

        //    if (students != null && students.Any())
        //    {
        //        var response = students.Select(Mapper.Mapper.GetStudentResponse);
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.OK;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.Status = HttpStatusCode.InternalServerError;
        //        serviceResponse.Result = Enumerable.Empty<StudentResponse>();
        //        serviceResponse.ErrorDetails = ResponseConstants.FailedStudentsFetch;
        //    }

        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<StudentResponse>> GetByEnrollmentNo(string enrollmentNo)
        //{
        //    var serviceResponse = new ServiceResponse<StudentResponse>();

        //    var student = await studentData.GetByEnrollmentNo(enrollmentNo);

        //    if (student != null)
        //    {
        //        var response = Mapper.Mapper.GetStudentResponse(student);
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.OK;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.ErrorDetails = ResponseConstants.NotFound;
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //    }

        //    return serviceResponse;

        //}

        //public async Task<ServiceResponse<StudentResponse>> GetById(int id)
        //{
        //    var serviceResponse = new ServiceResponse<StudentResponse>();
        //    var student = await studentData.GetById(id);

        //    if (student != null)
        //    {
        //        var response = Mapper.Mapper.GetStudentResponse(student);
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //        serviceResponse.Status = HttpStatusCode.OK;
        //    }
        //    else
        //    {
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //        serviceResponse.ErrorDetails = ResponseConstants.NotFound;
        //    }

        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<StudentResponse>> UpdateEnrollment(StudentRequest entity, int studentId)
        //{
        //    var serviceResponse = new ServiceResponse<StudentResponse>();
        //    var updatedStudent = await studentData.UpdateEnrollment(Mapper.Mapper.GetStudentEntity(entity), studentId);

        //    if (updatedStudent != null)
        //    {
        //        var response = Mapper.Mapper.GetStudentResponse(updatedStudent);
        //        serviceResponse.Status = HttpStatusCode.OK;
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //    }
        //    else
        //    {
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.ErrorDetails = ResponseConstants.NotFound;
        //    }

        //    return serviceResponse;
        //}


        //public async Task<ServiceResponse<IEnumerable<StatePercentageDto>>> GetStateStudentPercentage()
        //{
        //    var serviceResponse = new ServiceResponse<IEnumerable<StatePercentageDto>>();
        //    var statePercentages = await studentData.GetStateStudentPercentage();

        //    if (statePercentages.Any())
        //    {
        //        var response = statePercentages.Select(result => new StatePercentageDto
        //        {
        //            StateName = result.StateName,
        //            Percentage = result.Percentage
        //        });

        //        serviceResponse.Status = HttpStatusCode.OK;
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = response;
        //    }
        //    else
        //    {
        //        serviceResponse.Status = HttpStatusCode.NotFound;
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.ErrorDetails = ResponseConstants.NoStatePercentagesFoundError;
        //    }

        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<double>> GetSuccessfulJoinPercentage()
        //{

        //    var serviceResponse = new ServiceResponse<double>();

        //    double percentage = await studentData.GetSuccessfulJoinPercentage();

        //    if (percentage >= 0)
        //    {
        //        serviceResponse.IsSuccessful = true;
        //        serviceResponse.Result = percentage;
        //        serviceResponse.Status = HttpStatusCode.OK;
        //    }
        //    else
        //    {
        //        serviceResponse.Status = HttpStatusCode.InternalServerError;
        //        serviceResponse.IsSuccessful = false;
        //        serviceResponse.ErrorDetails = ResponseConstants.FetchingJoinPercentageError;
        //    }
        //    return serviceResponse;
        //}



    }

}


