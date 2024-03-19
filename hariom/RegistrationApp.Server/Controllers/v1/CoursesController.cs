using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.Course;
using RegistrationApp.Core.Services.Interface;
using RegistrationApp.Server.Extensions;

namespace RegistrationApp.Server.Controllers.v1
{
    /// <summary>
    /// Course controller for the course Apis
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "admin")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="courseService"></param>
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Create the Course 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateUpdateCourseModel model)
        {
            await _courseService.Create(model);
            return this.Response(StatusCodes.Status201Created, true, Message.Created, Result.Success);
        }

        /// <summary>
        /// Update Courses
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string id, CreateUpdateCourseModel model)
        {
            await _courseService.Update(id, model);
            return this.Response(StatusCodes.Status200OK, true, Message.Updated, Result.Success);
        }

        /// <summary>
        /// Delete Courses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            await _courseService.Delete(id);
            return this.Response(StatusCodes.Status200OK, true, Message.Deleted, Result.Success);
        }

        /// <summary>
        /// Get Course details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponse<CourseModel>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            return this.Response(StatusCodes.Status200OK, true, Message.DataFound, await _courseService.GetById(id));
        }

        /// <summary>
        /// Get all Course
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<CourseModel>>>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return this.Response(StatusCodes.Status200OK, true, Message.DataFound, await _courseService.GetAll());
        }
    }
}
