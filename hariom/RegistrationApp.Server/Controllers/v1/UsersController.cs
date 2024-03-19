using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.User;
using RegistrationApp.Core.Services.Interface;
using RegistrationApp.Server.Extensions;

namespace RegistrationApp.Server.Controllers.v1
{
    /// <summary>
    /// User Controller for the user Apis
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Regiter the student 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateUpdateUserModel model)
        {
            await _userService.Create(model);
            return this.Response(StatusCodes.Status201Created, true, Message.Created, Result.Success);
        }
        /// <summary>
        /// Update registration for students
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string id, CreateUpdateUserModel model)
        {
            await _userService.Update(id, model);
            return this.Response(StatusCodes.Status200OK, true, Message.Updated, Result.Success);

        }

        /// <summary>
        /// Delete Registration for students
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return this.Response(StatusCodes.Status200OK, true, Message.Deleted, Result.Success);
        }


        /// <summary>
        /// Get Rgistration details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<UserInfoModel>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            return this.Response(StatusCodes.Status200OK, true, Message.DataFound, await _userService.GetById(id));
        }

        /// <summary>
        /// Get all Rgistration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<UserInfoModel>>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return this.Response(StatusCodes.Status200OK, true, Message.DataFound, await _userService.GetAll());
        }
    }
}
