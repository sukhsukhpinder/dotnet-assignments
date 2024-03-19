using EnrollHub.Domain.Entities;
using EnrollHub.Domain.Repositories;
using EnrollHub.Infrastructure.StaticData;

namespace EnrollHub.Infrastructure.Repositories
{
    public class FileStudentRepository : IStudentRepository
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.FileName);

        public async Task<bool> DeleteEnrollmentById(string id)
        {
            // Read all lines from the file
            string[] lines = await File.ReadAllLinesAsync(filePath);
           var afterChanges = lines.ToList();
            bool deleted = false;

            // Iterate through each line

            foreach ( string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length > 0 && parts[0] == id)
                {
                    // Found the line with the given id, remove it
                    afterChanges.Remove(line);
                    deleted = true;
                    break;
                }
            }
            // Rewrite the file with the updated content
            await File.WriteAllLinesAsync(filePath, afterChanges);

            return deleted;
        }

        public Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> EnrollStudent(Student entity)
        {
            // If the file doesn't exist, create it and add headers
            if (!File.Exists(filePath))
            {
                string header = $"StudentId;EnrollmentNo;FirstName;LastName;DateOfBirth;Email;StateId;CourseId {Environment.NewLine}";
                await File.WriteAllTextAsync(filePath, header);
            }

            // Generate a new GUID for StudentId if it's not provided
            if (entity.StudentId == Guid.Empty)
            {
                entity.StudentId = Guid.NewGuid();
            }
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            // Construct the student record
            string studentRecord = $"{entity.StudentId};{entity.EnrollmentNo};{entity.FirstName};{entity.LastName};{entity.DateOfBirth};{entity.Email};{Data.states[entity.StateId]};{Data.courses[entity.CourseId]}{Environment.NewLine}";

            // Append the student record to the file
            await File.AppendAllTextAsync(filePath, studentRecord);
            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {

            var students = new List<Student>();
            // Read all lines from the file
            string[] lines = await File.ReadAllLinesAsync(filePath);

            // Iterate through each line to find the student with the matching id
            bool header = true;
            foreach (string line in lines)
            {
                if (header)
                {
                    header = false;
                    continue;
                }
                string[] fields = line.Split(';'); // Split the line into fields using semicolon (;) as the delimiter

                // Create a new Student object and populate its properties from the fields
                Student student = new Student
                {
                    StudentId = Guid.Parse(fields[0]),
                    EnrollmentNo = fields[1],
                    FirstName = fields[2],
                    LastName = fields[3],
                    DateOfBirth = string.IsNullOrEmpty(fields[4]) ? (DateTime?)null : DateTime.Parse(fields[4]),
                    Email = fields[5],
                    StateId = Data.FindKeyByValue(Data.states, fields[6]),
                    CourseId = Data.FindKeyByValue(Data.courses, fields[7]),
                };

                student.State = new States() { Id = student.StateId, Name = fields[6] };
                student.Course = new Course() { Id = student.CourseId, Name = fields[7] };

                students.Add(student);
            }

            return students;
        }

        public Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> GetById(string id)
        {
            // Read all lines from the file
            string[] lines = await File.ReadAllLinesAsync(filePath);

            // Iterate through each line to find the student with the matching id
            foreach (string line in lines)
            {
                string[] fields = line.Split(';'); // Split the line into fields using semicolon (;) as the delimiter

                // Check if the first field (StudentId) matches the provided id
                if (fields.Length > 0 && fields[0] == id)
                {
                    // Create a new Student object and populate its properties from the fields
                    Student student = new Student
                    {
                        StudentId = Guid.Parse(fields[0]),
                        EnrollmentNo = fields[1],
                        FirstName = fields[2],
                        LastName = fields[3],
                        DateOfBirth = string.IsNullOrEmpty(fields[4]) ? (DateTime?)null : DateTime.Parse(fields[4]),
                        Email = fields[5],
                        StateId = Data.FindKeyByValue(Data.states, fields[6]),
                        CourseId = Data.FindKeyByValue(Data.courses, fields[7])
                    };

                    return student; // Return the student object
                }
            }

            return new Student(); //record not found
        }


        public async Task<bool> UpdateEnrollment(Student entity, string id)
        {
           // Read all lines from the file
            string[] lines = await File.ReadAllLinesAsync(filePath);
            var afterChanges = lines.ToList();
            bool isUpdated = false;

            // Iterate through each line

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length > 0 && parts[0] == id)
                {
                    // Found the line with the given id, remove it
                    afterChanges.Remove(line);
                    string updatedStudent = $"{entity.StudentId};{parts[1]};{entity.FirstName};{entity.LastName};{entity.DateOfBirth};{entity.Email};{Data.states[entity.StateId]};{Data.courses[entity.CourseId]}";
                    afterChanges.Add(updatedStudent);
                    isUpdated = true;
                    break;
                }
            }
            // Rewrite the file with the updated content
            await File.WriteAllLinesAsync(filePath, afterChanges);

            return isUpdated;
        }

        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
