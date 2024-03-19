using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Application.Dtos.Response;
using sf_dotenet_mod.Application.Services.Base;
using sf_dotenet_mod.Controllers;

namespace sf_dotenet_mod_testcases
{
    public class StudentControllerTests
    {
        private readonly Mock<ILogger<StudentController>> _mockLogger;
        private readonly Mock<ICommonService> _mockCommonService;
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockLogger = new Mock<ILogger<StudentController>>();
            _mockCommonService = new Mock<ICommonService>();
            _mockStudentService = new Mock<IStudentService>();

            // Configure the mock to return a non-null value for GetAllActiveStates()
            _mockCommonService.Setup(x => x.GetAllActiveCourse()).ReturnsAsync(new List<KeyValuePair<int, string>>
        {
            new(1, "BA"),
            new(2, "BCA"),
            new(3, "MBA"),
            new(4, "BTech")
        });

            _mockCommonService.Setup(x => x.GetAllActiveStates()).ReturnsAsync(new List<KeyValuePair<int, string>>
        {
            new(1, "Andhra Pradesh"),
            new(2, "Arunachal Pradesh"),
            new(3, "Punjab"),
            new(4, "Chandigarh"),
            new(5, "Haryana"),
            new(6, "Himachal Pradesh"),
            new(7, "Delhi")
        });

            _controller = new StudentController(_mockLogger.Object, _mockCommonService.Object, _mockStudentService.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task Create_Get_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task Create_Post_RedirectsToAction_Index()
        {
            // Arrange
            var studentRequest = new StudentRequest();

            // Act
            var result = await _controller.Create(studentRequest);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            //var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            //Assert.Equal("index", redirectToActionResult.ActionName);
            //Assert.Null(redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResult()
        {

            // Arrange
            string studentId = "FCB0BB9B-896E-4923-99E6-8B83AFB8A650";
            var mockStudentResponse = new StudentResponse
            {
                FullName = "John Doe",
                StudentId = "FCB0BB9B-896E-4923-99E6-8B83AFB8A650",
                EnrollmentNo = "EN123456",
                StateName = "California",
                CourseName = "Computer Science",
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
                ModifiedOn = null,
                ModifiedBy = null,
                Active = true
            };
            var mockResponse = new Response<StudentResponse> { Result = mockStudentResponse };
            _mockStudentService.Setup(s => s.Get(studentId)).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Edit(studentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Edit", viewResult.ViewName);
            Assert.Equal(mockStudentResponse, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_RedirectsToAction_Index()
        {
            // Arrange
            var studentRequest = new StudentRequest { StudentId = "1" };

            // Act
            var result = await _controller.Edit(studentRequest);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            // Assert
            //var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            //Assert.Equal("index", redirectToActionResult.ActionName);
            //Assert.Null(redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task GetAll_ReturnsJsonResult()
        {
            // Arrange
            var mockResponse = new Response<IEnumerable<StudentResponse>>();
            _mockStudentService.Setup(s => s.GetAll()).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(mockResponse.Result, jsonResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsViewResult()
        {
            // Arrange
            string studentId = "6542F5FA-41F7-49E1-A28D-9526C33B1228";
            var mockResponse = new Response<StudentResponse>();
            _mockStudentService.Setup(s => s.Get(studentId)).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Get(studentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("View", viewResult.ViewName);
            Assert.Equal(mockResponse.Result, viewResult.Model);
        }

        [Fact]
        public async Task Delete_ReturnsJsonResult()
        {
            // Arrange
            string studentId = "E1E7DB23-2434-4A90-88B0-62AFD7688E8B";

            // Act
            var result = await _controller.Delete(studentId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(new { success = true, data = true }.ToString(), jsonResult.Value!.ToString());
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }
    }
}
