

using AuthenticationManager.Models;
using EntityFrameworkAPI.Models;
using EntityFrameworkAPI.Services.DTOs;

namespace EntityFrameworkAPI.Services.Services.Interface
{
    public interface IStudentService
    {
        Task<ResponseMetaData<IEnumerable<StudentDto>>> GetAll();

        Task<ResponseMetaData<AuthenticationResponse>> Authenticate(AuthenticationRequest request);

        Task<ResponseMetaData<StudentDto>> GetByIdAsync(int id);

        Task<ResponseMetaData<StudentDto>> GetByXmlIdAsync(string id);

        Task<ResponseMetaData<StudentDto>> Create(StudentDto studentDto);

        Task<ResponseMetaData<string>> Update(StudentDto studentDto);

        Task<ResponseMetaData<string>> Delete(int id);

        Task<ResponseMetaData<string>> RegisterUser(StudentDto userForRegistration);

        Task<ResponseMetaData<string>> ModifyUser(StudentDto userForRegistration);

        Task<ResponseMetaData<StudentDto>> GetByXmlNameAsync(string name);


    }
}
