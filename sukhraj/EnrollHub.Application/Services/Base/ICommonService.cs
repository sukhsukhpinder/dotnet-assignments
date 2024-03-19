
using EnrollHub.Application.Dtos.Model;
using Microsoft.Extensions.Caching.Memory;

namespace EnrollHub.Application.Services.Base
{
    public interface ICommonService
    {
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();
        Task<List<KeyValuePair<string, string>>> GetAllActiveUsers();
        Task<List<KeyValuePair<string, string>>> GetAllActiveRoles();
        Task<ChartData> GetStudentStateWise();
        Task<ChartData> GetStudentCourseWise();

    }
}
