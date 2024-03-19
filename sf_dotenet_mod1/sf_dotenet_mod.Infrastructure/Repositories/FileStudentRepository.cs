using Newtonsoft.Json;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class FileStudentRepository(ICommonRepository commonRepository) : IStudentRepository
    {
        private readonly string _filePath = Environment.CurrentDirectory + "\\StudentDb.txt";
        private readonly ICommonRepository _commonRepository = commonRepository;

        public async Task<bool> Delete(string id)
        {
            string data = File.ReadAllText(_filePath);
            var students = JsonConvert.DeserializeObject<List<Student>>(data);

            if (students != null && students.Count > 0)
            {
                var index = students.FindIndex(r => r.StudentId == Guid.Parse(id));
                if (index != -1)
                {
                    students.RemoveAt(index);
                    await File.WriteAllTextAsync(_filePath, JsonConvert.SerializeObject(students));
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            string data = File.ReadAllText(_filePath);
            var students = JsonConvert.DeserializeObject<List<Student>>(data);

            if (students != null && students.Count > 0)
            {
                var index = students.FindIndex(r => r.EnrollmentNo == enrollmentNo);
                if (index != -1)
                {
                    students.RemoveAt(index);
                    await File.WriteAllTextAsync(_filePath, JsonConvert.SerializeObject(students));
                    return true;
                }
            }
            return false;
        }

        public async Task<Student> Create(Student entity)
        {
            // If the file doesn't exist, create it and add headers
            if (!File.Exists(_filePath))
            {
                var myFile = File.Create(_filePath);
                myFile.Close();
            }

            // Generate a new GUID for StudentId if it's not provided
            //if (entity.StudentId == Guid.Empty)
            //{
            entity.StudentId = Guid.NewGuid();
            //}
            entity.EnrollmentNo = GenerateEnrollmentNumber();

            var studentList = (await GetAll()).ToList();
            studentList.Add(entity);
            string studentRecord = JsonConvert.SerializeObject(studentList);

            // Append the student record to the file
            await File.WriteAllTextAsync(_filePath, studentRecord);
            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            string data = await File.ReadAllTextAsync(_filePath);
            var students = JsonConvert.DeserializeObject<List<Student>>(data);
            if (students != null && students.Count > 0)
            {
                var states = _commonRepository.GetAllActiveStates();
                var courses = _commonRepository.GetAllActiveCourse();

                // Iterate over each student
                foreach (var student in students)
                {
                    // Bind State name to studentData
                    var state = states.Result.FirstOrDefault(s => s.Key == student.StateId);
                    student.State = new States { Id = state.Key, Name = state.Value };

                    // Bind Course name to studentData
                    var course = courses.Result.FirstOrDefault(c => c.Key == student.CourseId);
                    student.Course = new Course { Id = course.Key, Name = course.Value };
                }

                return students;
            }

            else return [];
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            string data = await File.ReadAllTextAsync(_filePath);
            var students = JsonConvert.DeserializeObject<List<Student>>(data);
            if (students != null && students.Count > 0)
            {
                var studentDetails = students.FirstOrDefault(s => s.EnrollmentNo == enrollmentNo);

                if (studentDetails != null)
                {
                    var states = _commonRepository.GetAllActiveStates();
                    var courses = _commonRepository.GetAllActiveCourse();

                    // Bind State name to studentData
                    var state = states.Result.FirstOrDefault(s => s.Key == studentDetails.StateId);
                    studentDetails.State = new States { Id = state.Key, Name = state.Value };

                    // Bind Course name to studentData
                    var course = courses.Result.FirstOrDefault(c => c.Key == studentDetails.CourseId);
                    studentDetails.Course = new Course { Id = course.Key, Name = course.Value };

                    return studentDetails; // Return the student object
                }
            }

            return new Student(); //record not found
        }

        public async Task<Student> Get(string id)
        {
            string data = await File.ReadAllTextAsync(_filePath);
            var students = JsonConvert.DeserializeObject<List<Student>>(data);
            if (students != null && students.Count > 0)
            {
                var studentDetails = students.FirstOrDefault(s => s.StudentId == Guid.Parse(id));
                if (studentDetails != null)
                {
                    var states = _commonRepository.GetAllActiveStates();
                    var courses = _commonRepository.GetAllActiveCourse();

                    // Bind State name to studentData
                    var state = states.Result.FirstOrDefault(s => s.Key == studentDetails.StateId);
                    studentDetails.State = new States { Id = state.Key, Name = state.Value };

                    // Bind Course name to studentData
                    var course = courses.Result.FirstOrDefault(c => c.Key == studentDetails.CourseId);
                    studentDetails.Course = new Course { Id = course.Key, Name = course.Value };
                    return studentDetails; // Return the student object
                }
            }

            return new Student(); //record not found
        }

        public async Task<List<KeyValuePair<string, int>>> GetChartDetails()
        {
            var studentData = await GetAll();
            if (studentData.Any())
            {
                var result = studentData
                            .GroupBy(s => s.State.Name)
                            .Select(g => new KeyValuePair<string, int>(g.Key, g.Count())).ToList();
                return result;
            }

            return [];
        }

        public async Task<Student> Update(Student entity, string id)
        {
            string data = File.ReadAllText(_filePath);
            if (data != null)
            {
                var students = JsonConvert.DeserializeObject<List<Student>>(data);
                if (students != null && students.Count > 0)
                {
                    int indexToUpdate = students.FindIndex(s => s.StudentId == Guid.Parse(id));

                    if (indexToUpdate != -1)
                    {
                        students[indexToUpdate] = entity;
                        await File.WriteAllTextAsync(_filePath, JsonConvert.SerializeObject(students));
                        return entity;
                    }
                }
            }
            return new Student(); //record not found
        }

        private static string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
