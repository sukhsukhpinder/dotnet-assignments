using EnrollHub.Domain.Repositories;
using EnrollHub.Infrastructure.StaticData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace EnrollHub.Infrastructure.Repositories
{
    public class FileCommonRepository : ICommonRepository
    {
        private readonly IServiceProvider _serviceProvider; 
        private readonly IStudentRepository _studentRepository; 
        public FileCommonRepository(IServiceProvider serviceProvider,IStudentRepository studentRepository)
        {
            _serviceProvider = serviceProvider;
            _studentRepository = studentRepository;
        }
        public  Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            List<KeyValuePair<int, string>> activeCourses = new List<KeyValuePair<int, string>>();

            foreach (var course in Data.courses)
            {
                activeCourses.Add(course);
            }

            return Task.FromResult(activeCourses);
        }
        public Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            List<KeyValuePair<int, string>> activeStates = new List<KeyValuePair<int, string>>();

            foreach (var state in Data.states)
            {
                activeStates.Add(state);
            }

            return Task.FromResult(activeStates);
        }

        public async Task<DataTable> GetStudentCourseWise()
        {
            var allStudents=await _studentRepository.GetAll();

            DataTable table = new DataTable();
            table.Columns.Add("CourseName");
            table.Columns.Add("TotalCount", typeof(int));

            foreach (var courseGroup in allStudents.GroupBy(x => x.Course.Name))
            {
                table.Rows.Add(courseGroup.Key, courseGroup.Count());
            }

            return await Task.FromResult(table);
        }

        public async Task<DataTable> GetStudentStateWise()
        {
            var fileStudentRepository = _serviceProvider.GetServices<IStudentRepository>()
                                                  .OfType<FileStudentRepository>()
                                                  .FirstOrDefault();

            var allStudents = await fileStudentRepository.GetAll();

            DataTable table = new DataTable();
            table.Columns.Add("StateName");
            table.Columns.Add("TotalCount", typeof(int));

            foreach (var satateGroup in allStudents.GroupBy(x => x.State.Name))
            {
                table.Rows.Add(satateGroup.Key, satateGroup.Count());
            }

            return await Task.FromResult(table);
        }
    }
}
