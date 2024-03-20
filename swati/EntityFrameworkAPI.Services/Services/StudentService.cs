
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
    public class StudentService : IStudentService
    {
        private readonly IUnit _unitOfWork;
        private readonly JwtTokenHandler _jwtTokenHandler;

        public StudentService(IUnit unitOfWork)
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
            var responseMetadata = new ResponseMetaData<AuthenticationResponse>();
            if (request == null) return responseMetadata;

            var authenticationResponse = await _jwtTokenHandler.GenerateJwtToken(request, null);

            if (authenticationResponse == null) { responseMetadata.Status = HttpStatusCode.Unauthorized; return responseMetadata; }

            responseMetadata.Result = authenticationResponse;
            responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;
        }

        /// <summary>
        /// sample
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="responseMetadata"></param>
        /// <returns></returns>
        private static ResponseMetaData<T> GetResponse<T>(T request, out ResponseMetaData<T> responseMetadata) where T : IComparable<T>
        {

            responseMetadata = new ResponseMetaData<T>()
            {
                Status = HttpStatusCode.OK,
                IsError = false,
            };
            if (request == null) { responseMetadata.IsError = true; responseMetadata.Status = HttpStatusCode.BadRequest; return responseMetadata; }
            return responseMetadata;
        }


        /// <summary>
        /// get all students
        /// </summary>
        /// <returns>ResponseMetaData<IEnumerable<StudentDto>></returns>
        public async Task<ResponseMetaData<IEnumerable<StudentDto>>> GetAll()
        {
            ResponseMetaData<IEnumerable<StudentDto>> responseMetadata = new();
            var list = await _unitOfWork.Students.GetAllAsync();

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
        public async Task<ResponseMetaData<StudentDto>> GetByIdAsync(int id)
        {
            var responseMetadata = new ResponseMetaData<StudentDto>();
            if (id <= 0) return responseMetadata;

            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = student.Adapt<StudentDto>();
            responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;
        }

        /// <summary>
        /// create student
        /// </summary>
        /// <param name="studentDto">StudentDto</param>
        /// <returns>ResponseMetaData<StudentDto></returns>
        public async Task<ResponseMetaData<StudentDto>> Create(StudentDto studentDto)
        {
            var responseMetadata = new ResponseMetaData<StudentDto>();

            if (studentDto == null) return responseMetadata;

            var student = studentDto.ToEntity();
            var result = await _unitOfWork.Students.AddAsync(student);
            if (result == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = studentDto; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;
        }

        /// <summary>
        /// update student
        /// </summary>
        /// <param name="studentDto">StudentDto</param>
        /// <returns>ResponseMetaData<string></returns>
        public async Task<ResponseMetaData<string>> Update(StudentDto studentDto)
        {
            var responseMetadata = new ResponseMetaData<string>();

            if (studentDto == null) return responseMetadata;

            Student student = studentDto.Adapt<Student>();
            var result = await _unitOfWork.Students.UpdateAsync(student);
            if (result == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;

        }

        /// <summary>
        /// delete student 
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ResponseMetaData<string></returns>
        public async Task<ResponseMetaData<string>> Delete(int id)
        {
            var responseMetadata = new ResponseMetaData<string>();

            if (id <= 0) return responseMetadata;

            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            await _unitOfWork.Students.DeleteAsync(student);
            responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
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

            //student = userForRegistration.ToEntity();

            var studentObj = await _unitOfWork.Students.AddAsync(student);
            if (studentObj == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

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

            Student student = await _unitOfWork.Students.GetByIdAsync(userForRegistration.StudentId);
            if (student == null) { responseMetadata.Status = HttpStatusCode.NotFound; return responseMetadata; }

            var sObj = mapper.Map<StudentDto, Student>(userForRegistration, student);

            var studentobj = await _unitOfWork.Students.UpdateAsync(sObj);
            responseMetadata.Result = "Success"; responseMetadata.IsError = false; responseMetadata.Status = HttpStatusCode.OK;
            return responseMetadata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ResponseMetaData<StudentDto>> GetByXmlIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseMetaData<StudentDto>> GetByXmlNameAsync(string name)
        {
            throw new NotImplementedException();
        }

    }
}
