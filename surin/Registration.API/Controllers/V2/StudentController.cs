using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Dtos;
using Registration.API.Services.Interface;
using System.Net;

namespace Registration.API.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/students")]    
    [ApiVersion("2")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICommonService _commonService;


        public StudentController(IStudentService studentService, ICommonService commonService)
        {
            _studentService = studentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Retrieve all students.
        /// </summary>
        /// <returns>The result containing a list of all students.</returns>        
        
        [HttpGet]
        [ProducesResponseType<ServiceResponse<IEnumerable<StudentResponse>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<IEnumerable<string>>>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<StudentResponse>>>> Get()
        {
            var response = await _studentService.GetAll();

            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieve a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>The result containing the requested student.</returns>

        [HttpGet("{id}")]
        [ProducesResponseType<ServiceResponse<StudentResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<StudentResponse>>> Get(int id)
        {
            var response = await _studentService.GetById(id);

            return StatusCode((int)response.Status, response);

        }

        /// <summary>
        /// Enroll a new student.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        /// POST /api/students
        /// {
        ///     "FirstName": "test",
        ///     "LastName": "test",
        ///     "DateOfBirth": "1990-01-01",
        ///     "Email": "test.test@example.com",
        ///     "StateId": 1,
        ///     "CourseId": 4,
        ///     "IsActive": true 
        /// }
        /// </remarks>
        /// <param name="request">The student details for enrollment.</param> 
        /// <returns>The result of the enrollment operation.</returns>


        [Authorize(Roles = "Admin")]
        [ProducesResponseType<ServiceResponse<StudentResponse>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<StudentResponse>>> Create([FromBody] StudentRequest request)
        {
            var response = await _studentService.EnrollStudent(request);

            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieve dropdown options for states and courses.
        /// </summary>
        /// <returns>Dropdown options for states and courses.</returns>

        [HttpGet("dropdown-options")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDropdownOptions()
        {
            var states = await _commonService.GetAllActiveStates();
            var courses = await _commonService.GetAllActiveCourse();

            return Ok(new { States = states, Courses = courses });
        }

        /// <summary>
        /// Delete a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student to be deleted.</param>
        /// <returns>The result of the deletion operation.</returns>

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]        
        [ProducesResponseType<ServiceResponse<bool>>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(int id)
        {
            var response = await _studentService.DeleteEnrollmentById(id);
            return StatusCode((int)response.Status, response);
        }
        /// <summary>
        /// Delete a student by enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student to be deleted.</param>
        /// <returns>The result of the deletion operation.</returns>

        [HttpDelete("by-enrollmentNo/{enrollmentNo}")]
        [ProducesResponseType<ServiceResponse<bool>>(StatusCodes.Status204NoContent)]        
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<bool>>> EnrollmentNumber(string enrollmentNo)
        {
            var response = await _studentService.DeleteEnrollmentByNumber(enrollmentNo);

            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieve a student by enrollment number.
        /// </summary>
        /// <param name="enrollmentNo">The enrollment number of the student to retrieve.</param>
        /// <returns>The result containing the requested student.</returns>

        [HttpGet("by-enrollmentNo/{enrollmentNo}")]
        [ProducesResponseType<ServiceResponse<StudentResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<StudentResponse>>> EnrollmentNo(string enrollmentNo)
        {

            var response = await _studentService.GetByEnrollmentNo(enrollmentNo);

            return Ok(response);

        }

        /// <summary>
        /// Update a student's enrollment details.
        /// </summary>
        /// <param name="studentId">The ID of the student to update.</param>
        /// <param name="entity">The updated student details.</param>
        /// <returns>The result of the update operation.</returns>

        [HttpPut("{studentId}")]
        [ProducesResponseType<ServiceResponse<StudentResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<StudentResponse>>> Update(int studentId, [FromBody] StudentRequest entity)
        {
            var response = await _studentService.UpdateEnrollment(entity, studentId);
            
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieve state-wise student enrollment percentages.
        /// </summary>
        /// <returns>The result containing state-wise student enrollment percentages.</returns>

        [HttpGet("state-percentage")]
        [ProducesResponseType<ServiceResponse<IEnumerable<StatePercentageDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<StatePercentageDto>>>> GetStateStudentPercentage()
        {
            var response = await _studentService.GetStateStudentPercentage();

            return Ok(response);

        }

        /// <summary>
        /// Retrieve the percentage of successfully joined students.
        /// </summary>
        /// <returns>The result containing the percentage of successfully joined students.</returns>

        [HttpGet("successful-joins")]
        [ProducesResponseType<ServiceResponse<double>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ServiceResponse<string>>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<double>>> GetSuccessfulJoinPercentage()
        {
            var response = await _studentService.GetSuccessfulJoinPercentage();
            return Ok(response);
        }

    }
}
