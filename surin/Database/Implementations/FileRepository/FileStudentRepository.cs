using Database.Contracts;
using Database.Entities;
using Newtonsoft.Json;

namespace Database.Implementations.FileRepository
{
    public class FileStudentRepository : IStudentContract
    {        
        string filePath = @"D:\Project\RegistrationSystem\RegistrationSystem\Database\FileDB\\FileStudentDb.txt";

        public async Task<Student> GetById(int id)
        {
            var students = await ReadStudentsFromFile();
            return students.FirstOrDefault(s => s.StudentId == id)!;
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            var students = await ReadStudentsFromFile();
            return students.FirstOrDefault(s => s.EnrollmentNo == enrollmentNo)!;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await ReadStudentsFromFile();
        }


        public async Task<Student> EnrollStudent(Student entity)
        {
            var students = await ReadStudentsFromFile();
            entity.StudentId = students.Count + 1;
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.isActive = entity.isActive || true;
            students.Add(entity);
            await WriteStudentsToFile(students);
            return entity;
        }

        public async Task<Student> UpdateEnrollment(Student entity, int id)
        {
            var students = await ReadStudentsFromFile();
            var existingStudent = students.FirstOrDefault(s => s.StudentId == id);

            if (existingStudent != null)
            {
                if (entity.FirstName != null)
                    existingStudent.FirstName = entity.FirstName;

                if (entity.LastName != null)
                    existingStudent.LastName = entity.LastName;

                if (entity.DateOfBirth != null)
                    existingStudent.DateOfBirth = entity.DateOfBirth;

                if (entity.Email != null)
                    existingStudent.Email = entity.Email;

                if (entity.StateId != 0)
                    existingStudent.StateId = entity.StateId;

                if (entity.CourseId != 0)
                    existingStudent.CourseId = entity.CourseId;

                existingStudent.isActive = entity.isActive;

                await WriteStudentsToFile(students);
            }

            return existingStudent!;
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var students = await ReadStudentsFromFile();
            var studentToRemove = students.FirstOrDefault(s => s.EnrollmentNo == enrollmentNo);

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                await WriteStudentsToFile(students);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteEnrollmentById(int id)
        {
            var students = await ReadStudentsFromFile();
            var studentToRemove = students.FirstOrDefault(s => s.StudentId == id);

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                await WriteStudentsToFile(students);
                return true;
            }

            return false;
        }

        public async Task<double> GetSuccessfulJoinPercentage()
        {
            var allStudents = await GetAll();
            var successfullyJoined = allStudents.Select(x => x.isActive).Count();

            if (allStudents.Any())
            {
                double successPercentage = (double)successfullyJoined / allStudents.Count() * 100;
                return successPercentage;
            }

            return 0;
        }

        public async Task<IEnumerable<(string StateName, double Percentage)>> GetStateStudentPercentage()
        {
            var allStudents = await GetAll();

            if (allStudents.Any())
            {
                var statePercentages = allStudents
                    .GroupBy(s => s.State!.Name)
                    .Select(group => new
                    {
                        StateName = group.Key,
                        Percentage = (double)group.Count() / allStudents.Count() * 100
                    })
                    .Select(result => (result.StateName, result.Percentage));

                return statePercentages!;
            }

            return new List<(string StateName, double Percentage)>();
        }


        private async Task<List<Student>> ReadStudentsFromFile()
        {
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }

            return new List<Student>();
        }
        private async Task WriteStudentsToFile(List<Student> students)
        {
            var json = JsonConvert.SerializeObject(students, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);
        }

        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
