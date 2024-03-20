using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWebAppAPI.Helpers;
using SchoolWebAppAPI.Models;
using StudentRegistrationApi.Interfaces;

namespace SchoolWebAppAPI.Controllers.v2
{
    /// <summary>
    /// Contains methods related to token
    /// </summary>

    [ApiVersion("2.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [ValidateModel]
   
    public class TokenController : ControllerBase
    {
        private readonly IStudentData _studentData;
        /// <summary>
        /// TokenController constructor
        /// </summary>
        /// <param name="studentData"></param>
        public TokenController(IStudentData studentData)
        {
            _studentData = studentData;
        }
        /// <summary>
        /// To refresh user token
        /// </summary>
        /// <param name="tokenApiDto"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<ResponseModel<Tokens>> refresh([FromBody] TokenApiDto tokenApiDto)
        {
            return await _studentData.RefreshToken(tokenApiDto);


        }
    }
}
