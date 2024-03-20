
using AuthenticationManager.Models;
using EntityFrameworkAPI.Models;
using EntityFrameworkAPI.Services.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkAPI.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IStudentService studentService, ILogger<AuthenticationController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ResponseMetaData<AuthenticationResponse>> Login([FromBody] AuthenticationRequest request)
        {
            var responseMetadata = _studentService.Authenticate(request);
            return await responseMetadata;
        }
    }
}
