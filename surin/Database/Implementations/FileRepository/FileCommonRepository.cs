using Database.Contracts;
using Database.Entities;
using Newtonsoft.Json;

namespace Database.Implementations.FileRepository
{
    public class FileCommonRepository : ICommonContract
    {
        private static string filePath = @"D:\Project\RegistrationSystem\RegistrationSystem\Database\FileDB";
        private readonly string coursesFilePath = filePath + "\\FileCoursesDb.txt";
        private readonly string statesFilePath = filePath + "\\FileStatesDb.txt";       

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            var courses = await ReadCoursesFromFile();
            return courses
                .Where(c => c.isActive)
                .Select(c => new KeyValuePair<int, string>(c.Id, c.Name!))
                .ToList();
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            var states = await ReadStatesFromFile();
            return states
                .Where(s => s.isActive)
                .Select(s => new KeyValuePair<int, string>(s.Id, s.Name!))
                .ToList();
        }

        private async Task<List<Course>> ReadCoursesFromFile()
        {
            if (File.Exists(coursesFilePath))
            {
                var json = await File.ReadAllTextAsync(coursesFilePath);
                return JsonConvert.DeserializeObject<List<Course>>(json) ?? new List<Course>();
            }
            else
            {

                var initialCourseData = new List<Course>
            {
                new Course { Id = 1, Name = "BA", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new Course { Id = 2, Name = "BCA", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new Course { Id = 3, Name = "MBA", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new Course { Id = 4, Name = "BTech", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true }
            };

                await File.WriteAllTextAsync(coursesFilePath, JsonConvert.SerializeObject(initialCourseData, Formatting.Indented));
                return initialCourseData;
            }
        }

        private async Task<List<States>> ReadStatesFromFile()
        {
            if (File.Exists(statesFilePath))
            {
                var json = await File.ReadAllTextAsync(statesFilePath);
                return JsonConvert.DeserializeObject<List<States>>(json) ?? new List<States>();
            }
            else
            {

                var initialStateData = new List<States>
            {
                new States { Id = 1, Name = "Andhra Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 2, Name = "Arunachal Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 3, Name = "Punjab", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 4, Name = "Chandigarh", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 5, Name = "Haryana", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 6, Name = "Himachal Pradesh", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true },
                new States { Id = 7, Name = "Delhi", CreatedOn = DateTime.UtcNow, CreatedBy = "", isActive = true }
            };

                await File.WriteAllTextAsync(statesFilePath, JsonConvert.SerializeObject(initialStateData, Formatting.Indented));
                return initialStateData;
            }
        }
    }
}
