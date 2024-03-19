using System;
using System.Data;

namespace EnrollHub.Domain.Repositories
{
    public interface ICommonRepository
    {
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();

        Task<DataTable> GetStudentStateWise();
        Task<DataTable> GetStudentCourseWise();
    }
}
