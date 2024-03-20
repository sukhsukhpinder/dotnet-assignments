using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWebAppAPI.Helpers;
using SchoolWebAppAPI.Models;
using StudentRegistrationApi.Interfaces;
using System.Net;

namespace SchoolWebAppAPI.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [ValidateModel] 
    public class StudentController : ControllerBase
    {
        private readonly IStudentData _studentData;
        private readonly ILog _logger;

        public StudentController(IStudentData studentData, ILog logger)
        {
            _studentData = studentData;
            _logger = logger;
        }
        /// <summary>
        /// To register student into website
        /// </summary>
        /// <param name="userObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ResponseModel<string>> add([FromBody] User userObj)
        {
            
            return await _studentData.AddStudent(userObj);
        }

        /// <summary>
        /// For logging in student
        /// </summary>
        /// <param name="userObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ResponseModel<Tokens> login([FromBody] LoginUser userObj)
        {
            return new ResponseModel<Tokens>()
            {
                IsSuccessful = true,
                Message = "StudentLogin from v2 done  "
            };
        }

      

    }
}
