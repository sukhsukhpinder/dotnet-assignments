using EnrollHub.Application.Dtos.Model;
using EnrollHub.Application.Services.Base;
using EnrollHub.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EnrollHub.Application.Services
{
    /// <summary>
    /// Service class providing common functionalities related to enrollment operations.
    /// </summary>
    public class CommonService: ICommonService
    {   
        private readonly ICommonRepository _commonRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMemoryCache _cache;

        public CommonService(ICommonRepository commonRepository, IUsersRepository usersRepository, IMemoryCache memoryCache)
        {
            _commonRepository= commonRepository;
            _usersRepository=usersRepository;
            _cache = memoryCache;
        }

        /// <summary>
        /// Retrieves all active states asynchronously.
        /// </summary>
        /// <returns>A list of key-value pairs representing active states.</returns>

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            string cacheKey = "ActiveStates";
            if (!_cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> states))
            {
                states =  await _commonRepository.GetAllActiveStates();
                _cache.Set(cacheKey, states, TimeSpan.FromMinutes(10));
            }
            return states;
        }
        /// <summary>
        /// Retrieves all active courses asynchronously.
        /// </summary>
        /// <returns>A list of key-value pairs representing active courses.</returns>

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            string cacheKey = "ActiveCourses";
            if (!_cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> courses))
            {
                courses = await _commonRepository.GetAllActiveCourse();
                _cache.Set(cacheKey, courses, TimeSpan.FromMinutes(10));
            }
            return courses;
        }
        /// <summary>
        /// Retrieves all active users asynchronously.
        /// </summary>
        /// <returns>A list of key-value pairs representing active users.</returns>

        public async Task<List<KeyValuePair<string, string>>> GetAllActiveUsers()
        {
               return await _usersRepository.GetAllActiveUsers();
        }
        /// <summary>
        /// Retrieves all active roles asynchronously.
        /// </summary>
        /// <returns>A list of key-value pairs representing active roles.</returns>

        public async Task<List<KeyValuePair<string, string>>> GetAllActiveRoles()
        {
                return  await _usersRepository.GetAllActiveRoles();
        }

        /// <summary>
        /// Retrieves student state-wise data asynchronously.
        /// </summary>
        /// <returns>ChartData object representing student state-wise data.</returns>


        public async Task<ChartData> GetStudentStateWise()
        {

            var table = await _commonRepository.GetStudentStateWise();
            
            var states = table.AsEnumerable().Select(x => Convert.ToString(x["StateName"])).Distinct().ToList();

            ChartData chartData = new ChartData();

            Dataset dataSetTotal = new Dataset();

            foreach (var stateName in states)
            {
                chartData.labels.Add(stateName);

                var totalCount = table.AsEnumerable().Where(x => Convert.ToString(x["StateName"]) == stateName)
                    .Sum(x => Convert.ToInt32(x["TotalCount"])).ToString();

                dataSetTotal.data.Add(totalCount);
            }
            
            chartData.datasets.Add(dataSetTotal);

            return chartData;
        }

        /// <summary>
        /// Retrieves student course-wise data asynchronously.
        /// </summary>
        /// <returns>ChartData object representing student course-wise data.</returns>

        public async Task<ChartData> GetStudentCourseWise()
        {

            var tableCourse = await _commonRepository.GetStudentCourseWise();

            var courses = tableCourse.AsEnumerable().Select(x => Convert.ToString(x["CourseName"])).Distinct().ToList();

            ChartData chartData = new ChartData();


            Dataset dataSetStudentCourseWise = new Dataset();
            dataSetStudentCourseWise.label = "Course Chart";


                foreach (var courseName in courses)
                {
                    chartData.labels.Add(courseName);

                    var totalCountCourse = tableCourse.AsEnumerable().Where(x => Convert.ToString(x["CourseName"]) == courseName)
                        .Sum(x => Convert.ToInt32(x["TotalCount"])).ToString();
                    dataSetStudentCourseWise.data.Add(totalCountCourse);
                }

            chartData.datasets.Add(dataSetStudentCourseWise);

            return chartData;
        }


    }
}
