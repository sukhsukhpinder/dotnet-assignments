
using EntityFrameworkAPI.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using EntityFrameworkAPI.Core.Helpers;
using Mapster;
using System.Net;
using EntityFrameworkAPI.Models;
using EntityFrameworkAPI.Services.Services.Interface;
using EntityFrameworkAPI.Services.DTOs;
using AuthenticationManager.Models;
using EntityFrameworkAPI.Enums;
using AuthenticationManager.Data;
using Microsoft.AspNetCore.Authorization;
using EntityFrameworkAPI.Services.Handlers;
using EntityFrameworkAPI.Core.UnitOfWork;

namespace EntityFrameworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        public StudentController(IUnit unitOfWork, IXMLUnit xmlUnitOfWork, ILogger<StudentController> logger)
        {
            _studentService = new ServiceResolverHandler(unitOfWork, xmlUnitOfWork).GetServiceResolver();
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ResponseMetaData<AuthenticationResponse>> Login([FromBody] AuthenticationRequest request)
        {
            var responseMetadata = _studentService.Authenticate(request);
            return await responseMetadata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> Get()
        {
            var responseMetadata = await _studentService.GetAll();
            this._logger.LogInformation("Get all students");
            
            return StatusCode((int)responseMetadata.Status, responseMetadata);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDto>> Get(int id)
        {
            var responseMetadata = await _studentService.GetByIdAsync(id);
            this._logger.LogInformation("Get student by student id");
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(StudentDto studentDto)
        {
            this._logger.LogInformation("create student object");
            var responseMetadata = await _studentService.Create(studentDto);
            
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(StudentDto sDto)
        {
            this._logger.LogInformation("update student object");
            var responseMetadata = await _studentService.Update(sDto);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            this._logger.LogInformation("delete student object");
            var responseMetadata = await _studentService.Delete(id);
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
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] StudentDto userForRegistration)
        {
            this._logger.LogInformation("register student object");
            var responseMetadata = await _studentService.RegisterUser(userForRegistration);
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modify")]
        public async Task<IActionResult> ModifyUser([FromBody] StudentDto userForRegistration)
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
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("info")]
        public async Task<ActionResult<StudentDto>> GetXMLStudent(string id, string name)
        {
            var responseMetadata = await _studentService.GetByXmlIdAsync(id);
            this._logger.LogInformation("Get student by student id");
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

    }
}
