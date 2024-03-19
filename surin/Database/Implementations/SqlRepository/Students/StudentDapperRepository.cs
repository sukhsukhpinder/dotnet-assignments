using Dapper;
using Database.Contracts;
using Database.Entities;
using System.Data;

namespace Database.Implementations.SqlRepository.Students
{
    public class StudentDapperRepository : IStudentContract
    {
        private readonly IDbConnection dbConnection;

        public StudentDapperRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<bool> DeleteEnrollmentById(int id)
        {
            var sql = "DELETE FROM Students WHERE StudentId = @Id";
            var affectedRows = await dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var sql = "DELETE FROM Students WHERE EnrollmentNo = @EnrollmentNo";
            var affectedRows = await dbConnection.ExecuteAsync(sql, new { EnrollmentNo = enrollmentNo });
            return affectedRows > 0;
        }

        public async Task<Student> EnrollStudent(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.isActive = entity.isActive || true;

            var sql = @"INSERT INTO Students (EnrollmentNo, FirstName, LastName, DateOfBirth, Email, StateId, CourseId, isActive)
                    VALUES (@EnrollmentNo, @FirstName, @LastName, @DateOfBirth, @Email, @StateId, @CourseId, @isActive);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

            var studentId = await dbConnection.QueryFirstOrDefaultAsync<int>(sql, entity);
            entity.StudentId = studentId;

            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            var sql = "SELECT * FROM Students";
            return await dbConnection.QueryAsync<Student>(sql);
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            var sql = "SELECT * FROM Students WHERE EnrollmentNo = @EnrollmentNo";
            return await dbConnection.QueryFirstOrDefaultAsync<Student>(sql, new { EnrollmentNo = enrollmentNo });
        }

        public async Task<Student> GetById(int id)
        {
            var sql = "SELECT * FROM Students WHERE StudentId = @Id";
            return await dbConnection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
        }

        public async Task<Student> UpdateEnrollment(Student entity, int id)
        {
            var sql = @"UPDATE Students 
                    SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Email = @Email, 
                        StateId = @StateId, CourseId = @CourseId, isActive = @isActive
                    WHERE StudentId = @Id";

            entity.StudentId = id;
            await dbConnection.ExecuteAsync(sql, entity);

            return entity;
        }

        public async Task<IEnumerable<(string StateName, double Percentage)>> GetStateStudentPercentage()
        {
            var sql = @"SELECT s.Name as StateName, COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Students) as Percentage
                    FROM Students st
                    INNER JOIN States s ON st.StateId = s.Id
                    GROUP BY s.Name";

            return await dbConnection.QueryAsync<(string, double)>(sql);
        }

        public async Task<double> GetSuccessfulJoinPercentage()
        {
            var sql = "SELECT COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Students) FROM Students WHERE isActive = 1";
            return await dbConnection.QueryFirstOrDefaultAsync<double>(sql);
        }

        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
