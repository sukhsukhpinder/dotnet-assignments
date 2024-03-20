
using AuthenticationManager.Models;
using EntityFrameworkAPI.Core.Entities;
using EntityFrameworkAPI.Core.Helpers;
using EntityFrameworkAPI.Core.UnitOfWork;
using EntityFrameworkAPI.Models;
using EntityFrameworkAPI.Services.DTOs;
using EntityFrameworkAPI.Services.Handlers;
using EntityFrameworkAPI.Services.Services.Interface;
using Mapster;
using MapsterMapper;
using System.Net;

namespace EntityFrameworkAPI.Services.Services
{
    public class XMLService : IStudentService
    {
        private readonly IXMLUnit _unitOfWork;
        private readonly JwtTokenHandler _jwtTokenHandler;
        public XMLService(IXMLUnit unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenHandler = new JwtTokenHandler();
        }

        /// <summary>
        /// Authenticate student
        /// </summary>
        /// <param name="request">AuthenticationRequest</param>
        /// <returns>ResponseMetaData<AuthenticationResponse></returns>
        public async Task<ResponseMetaData<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        {
            var authenticationResponse = new AuthenticationResponse();
            var responseMetadata = new ResponseMetaData<AuthenticationResponse>()
            {
                Status = HttpStatusCode.BadRequest,
                IsError = true,
            };
            try
            {
                if (request == null) return responseMetadata;

                var student = _unitOfWork.XMLStudents.DeserializeXmlFileToObject(request.UserName);
                if (student == null) return responseMetadata;

                authenticationResponse = new AuthenticationResponse
                {
                    UserId = _unitOfWork.XMLStudents.GetStudentId(request.UserName),
                    Role = student.Role
                };

                var isVerified = PasswordHasher.VerifyPassword(request.Password, student.PasswordHash);
                if (!isVerified) { return responseMetadata; }

                authenticationResponse = await _jwtTokenHandler.GenerateJwtToken(request, authenticationResponse);

                if (authenticationResponse == null) { responseMetadata.Status = HttpStatusCode.Unauthorized; return responseMetadata; }

                responseMetadata.Result = authenticationResponse;
                responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
                return responseMetadata;
            }
            catch (Exception ex) { responseMetadata.ErrorDetails = ex.Message; return responseMetadata; }
        }

        /// <summary>
        /// get all students
        /// </summary>
        /// <returns>ResponseMetaData<IEnumerable<StudentDto>></returns>
        public async Task<ResponseMetaData<IEnumerable<StudentDto>>> GetAll()
        {
            var responseMetadata = new ResponseMetaData<IEnumerable<StudentDto>>();
            var list = _unitOfWork.XMLStudents.DeserializeAllXmlFileToList();

            if (list == null) { responseMetadata.IsError = true; responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = list.AsQueryable().ProjectToType<StudentDto>().ToList();
            responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        /// <summary>
        /// get student by student id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ResponseMetaData<StudentDto></returns>
        public async Task<ResponseMetaData<StudentDto>> GetByXmlIdAsync(string id)
        {
            var responseMetadata = new ResponseMetaData<StudentDto>();
            if (id.ToString() == "0") return responseMetadata;

            var student = _unitOfWork.XMLStudents.DeserializeXmlFileToObjectById(id);

            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = student.Adapt<StudentDto>();
            responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        /// <summary>
        /// get student by student id
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>ResponseMetaData<StudentDto></returns>
        public async Task<ResponseMetaData<StudentDto>> GetByXmlNameAsync(string name)
        {
            var responseMetadata = new ResponseMetaData<StudentDto>();
            if (string.IsNullOrEmpty(name)) return responseMetadata;

            var student = _unitOfWork.XMLStudents.DeserializeXmlFileToObject(name);

            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = student.Adapt<StudentDto>();
            responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        /// <summary>
        /// register new student
        /// </summary>
        /// <param name="userForRegistration">StudentDto</param>
        /// <returns>ResponseMetaData<string></returns>
        public async Task<ResponseMetaData<string>> RegisterUser(StudentDto userForRegistration)
        {
            var responseMetadata = new ResponseMetaData<string>();
            if (userForRegistration == null && userForRegistration.Password == null) return responseMetadata;

            var student = new Student();

            var hashedPassword = PasswordHasher.HashPassword(userForRegistration.Password);
            student.PasswordHash = hashedPassword;

            student.StudentName = userForRegistration.StudentName;
            student.Email = userForRegistration.Email;
            student.DateOfBirth = userForRegistration.DateOfBirth;
            student.Role = userForRegistration.Role;
            student.JoinedDate = userForRegistration.JoinedDate;

            var result = _unitOfWork.XMLStudents.SerializeObjectToXmlFile(student, Guid.NewGuid().ToString(), student.StudentName);

            if (!result) { responseMetadata.Status = HttpStatusCode.InternalServerError; return responseMetadata; }

            responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;


        }

        /// <summary>
        /// modify student
        /// </summary>
        /// <param name="userForRegistration">StudentDto</param>
        /// <returns>ResponseMetaData<string></returns>
        public async Task<ResponseMetaData<string>> ModifyUser(StudentDto userForRegistration)
        {
            Mapper mapper = new Mapper();
            var responseMetadata = new ResponseMetaData<string>();
            if (userForRegistration == null && userForRegistration.Password == null) return responseMetadata;
            var studentGuid = userForRegistration.XmlStudentId;
            var student = _unitOfWork.XMLStudents.DeserializeXmlFileToObjectById(studentGuid);

            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            var sObj = mapper.Map<StudentDto, Student>(userForRegistration, student);

            var studentobj = _unitOfWork.XMLStudents.SerializeObjectToXmlFile(sObj, studentGuid, sObj.StudentName);
            responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        public Task<ResponseMetaData<StudentDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentDto"></param>
        /// <returns></returns>
        public async Task<ResponseMetaData<StudentDto>> Create(StudentDto studentDto)
        {
            var responseMetadata = new ResponseMetaData<StudentDto>();
            if (studentDto == null) return responseMetadata;

            var student = studentDto.ToEntity();

            studentDto.XmlStudentId = Guid.NewGuid().ToString();
            var result = _unitOfWork.XMLStudents.SerializeObjectToXmlFile(student, studentDto.XmlStudentId, studentDto.StudentName);

            if (result) { responseMetadata.Status = HttpStatusCode.InternalServerError; return responseMetadata; }

            responseMetadata.Result = studentDto; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentDto"></param>
        /// <returns></returns>
        public async Task<ResponseMetaData<string>> Update(StudentDto studentDto)
        {
            var responseMetadata = new ResponseMetaData<string>()
            {
                Status = HttpStatusCode.BadRequest,
                IsError = true,
            };
            try
            {
                if (studentDto == null) return responseMetadata;

                Student student = studentDto.Adapt<Student>();
                //var result = await _unitOfWork.XMLStudents.UpdateAsync(student);
                //if (result == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

                responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
                return responseMetadata;
            }
            catch (Exception ex) { responseMetadata.ErrorDetails = ex.Message; return responseMetadata; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseMetaData<string>> Delete(int id)
        {
            var responseMetadata = new ResponseMetaData<string>()
            {
                Status = HttpStatusCode.BadRequest,
                IsError = true,
            };
            try
            {
                if (id <= 0) return responseMetadata;

                var filePath = System.IO.Directory.GetFiles(@"Resources/", id.ToString());
                if (System.IO.File.Exists(filePath[0]))
                {
                    System.IO.File.Delete(filePath[0]);
                    responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
                    return responseMetadata;
                }
                else responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata;
            }
            catch (Exception ex) { responseMetadata.ErrorDetails = ex.Message; responseMetadata.Status = HttpStatusCode.BadRequest; return responseMetadata; }

        }
    }
}
