
using Asp.Versioning;
using AuthenticationManager.Data;
using AuthenticationManager.Models;
using EntityFrameworkAPI.Models;
using EntityFrameworkAPI.Services.DTOs;
using EntityFrameworkAPI.Services.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using static EntityFrameworkAPI.Extensions.ServiceConfig;

namespace EntityFrameworkAPI.Controllers.v1
{
    /// <summary>
    /// Student request and response
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger, ServiceResolver serviceResolver)
        {
            var config = ConfigSettings.StorageProvider;
            _studentService = serviceResolver(config);
            _logger = logger;
        }

        /// <summary>
        /// Authenticate student
        /// </summary>
        /// <param name="request">AuthenticationRequest</param>
        /// <returns>ResponseMetaData<AuthenticationResponse></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ResponseMetaData<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            var responseMetadata = _studentService.Authenticate(request);
            return await responseMetadata;
        }

        /// <summary>
        /// Register new student in student board
        /// </summary>
        /// <param name="userForRegistration">StudentDto</param>
        /// <returns>Response data with StudentDto</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(StudentDto userForRegistration)
        {
            this._logger.LogInformation("register student object");
            var responseMetadata = await _studentService.RegisterUser(userForRegistration);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>Response data with IEnumerable<StudentDto></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> Get()
        {
            var responseMetadata = await _studentService.GetAll();
            this._logger.LogInformation("Get all students");

            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>StudentDto</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDto>> Get(int id)
        {
            var responseMetadata = await _studentService.GetByIdAsync(id);
            this._logger.LogInformation("Get student by student id");
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// Deleting student by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Response data</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            this._logger.LogInformation("delete student object");
            var responseMetadata = await _studentService.Delete(id);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// Update student details
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modify")]
        public async Task<IActionResult> ModifyUser(StudentDto userForRegistration)
        {
            var responseMetadata = await _studentService.ModifyUser(userForRegistration);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        //[HttpPost]
        //[Route("authenticate")]
        //public async Task<ActionResult<AuthenticationResponse?>> Authenticate([FromBody] AuthenticationRequest request)
        //{
        //    if (request == null) return BadRequest();

        //    var authenticationResponse = await _jwtTokenHandler.GenerateJwtToken(request);
        //    if (authenticationResponse == null) { return Unauthorized(); }
        //    return Ok(new
        //    { authenticationResponse });
        //}
        /// <summary>
        /// get student by id for xml file system
        /// </summary>
        /// <param name="studentId">string</param>
        /// <returns>StudentDto</returns>
        [HttpGet]
        [Route("info")]
        public async Task<ActionResult<StudentDto>> GetXMLStudent(string id, string name)
        {
            var responseMetadata = await _studentService.GetByXmlIdAsync(id);
            this._logger.LogInformation("Get student by student id");
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok(new { fullPath });
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Create new student entity
        /// </summary>
        /// <param name="student">StudentDto</param>
        /// <returns>StudentDto</returns>
        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(StudentDto studentDto)
        {
            this._logger.LogInformation("create student object");
            var responseMetadata = await _studentService.Create(studentDto);

            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="student">StudentDto</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(StudentDto sDto)
        {
            this._logger.LogInformation("update student object");
            var responseMetadata = await _studentService.Update(sDto);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }
    }
}
