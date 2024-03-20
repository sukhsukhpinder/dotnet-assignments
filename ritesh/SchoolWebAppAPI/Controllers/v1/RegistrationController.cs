using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWebAppAPI.Interfaces;
using SchoolWebAppAPI.Models;
using StudentRegistrationApi.Interfaces;
using System.Text.RegularExpressions;

namespace SchoolWebAppAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [Authorize]

    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationData _registrationData;

        public RegistrationController(IRegistrationData registrationData)
        {
            _registrationData = registrationData;
        }
        /// <summary>
        /// Method to GetStates to bind UI dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("states")]
        public async Task<ResponseModel<IList<States>>> states()
        {
            var list = await _registrationData.GetStates();
            return new ResponseModel<IList<States>>()
            {
                IsSuccessful = true,
                Data = list,
                Message = ResponseMessages.GetData
            };
        }
        /// <summary>
        /// To submit Registration data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Registration obj</returns>
        [HttpPost]
        [Route("submit")]
        public async Task<ResponseModel<string>> submit(Registration obj)
        {
            return await _registrationData.SaveRegistration(obj);
        }
        /// <summary>
        /// To get list of submitted Registration
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of submitted Registrations</returns>
        [HttpGet]
        [Route("all")]
        public async Task<ResponseModel<IList<Registration>>> all(int UserId)
        {
            var regList= await _registrationData.GetRegistrations(UserId);
            return new ResponseModel<IList<Registration>>
            {
                IsSuccessful = true,
                Message = ResponseMessages.GetData,
                Data = regList
            };
        }
        /// <summary>
        /// To get dashboard chart data 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>DashboardData obj</returns>
        [HttpGet]
        [Route("dashboard")]
        public async Task<ResponseModel<DashboardData>> dashboard(int UserId)
        {
            var dashData= await _registrationData.GetDashboardData(UserId);
            return new ResponseModel<DashboardData>
            {
                IsSuccessful = true,
                Message = ResponseMessages.GetData,
                Data = dashData
            };
        }
        /// <summary>
        /// to get submitted Registration details
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("detail")]
        public async Task<ResponseModel<Registration>>detail(int UserId, int RegId)
        {
            var regDetail = await _registrationData.GetRegistrationDetail(UserId, RegId);
            return new ResponseModel<Registration>
            {
                IsSuccessful = true,
                Message = ResponseMessages.GetData,
                Data = regDetail
            };
        }
        /// <summary>
        /// To delete submitted registration
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("inactive")]
        public async Task<ResponseModel<string>> inactive(int UserId, int RegId)
        {
            return await _registrationData.DeleteRegistration(UserId, RegId);
        }

    }

}
