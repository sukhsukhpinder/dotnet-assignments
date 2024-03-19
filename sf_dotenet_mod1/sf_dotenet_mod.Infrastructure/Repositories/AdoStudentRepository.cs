using Microsoft.Data.SqlClient;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class AdoStudentRepository(SqlConnectionFactory connection) : IStudentRepository
    {
        private readonly SqlConnectionFactory _connection = connection;

        public async Task<Student> Create(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.Active = entity.Active || true;

            using SqlConnection connection = _connection.CreateConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            await EnrollStudent(entity, connection, transaction);

            transaction.Commit();

            return entity;
        }

        private static async Task<int> EnrollStudent(Student entity, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand(QueryConstants.InsertStudent, connection, transaction);
            command.Parameters.AddWithValue("@StudentId", Guid.NewGuid());
            command.Parameters.AddWithValue("@FirstName", entity.FirstName);
            command.Parameters.AddWithValue("@LastName", entity.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@StateId", entity.StateId);
            command.Parameters.AddWithValue("@CourseId", entity.CourseId);
            command.Parameters.AddWithValue("@EnrollmentNo", GenerateEnrollmentNumber());
            command.Parameters.AddWithValue("@Active", entity.Active);

            var result = await command.ExecuteNonQueryAsync();

            return result;
        }

        public async Task<bool> Delete(string id)
        {
            using SqlConnection connection = _connection.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new(QueryConstants.DeleteStudent, connection);
            command.Parameters.AddWithValue("@StudentId", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            using SqlConnection connection = _connection.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new(QueryConstants.DeleteStudentByEnrollment, connection);
            command.Parameters.AddWithValue("@EnrollmentNo", enrollmentNo);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<Student> Get(string id)
        {
            var student = new Student();

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using SqlCommand command = new(QueryConstants.GetStudentById, connection);
                command.Parameters.AddWithValue("@StudentId", id);

                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {

                    student.StudentId = (Guid)reader["StudentId"];
                    student.EnrollmentNo = reader["EnrollmentNo"].ToString()!;
                    student.FirstName = reader["FirstName"].ToString()!;
                    student.LastName = reader["LastName"].ToString()!;
                    student.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null;
                    student.Email = reader["Email"].ToString()!;
                    student.StateId = Convert.ToInt32(reader["StateId"]);
                    student.CourseId = Convert.ToInt32(reader["CourseId"]);
                    student.Course = new Course() { Name = reader["CourseName"].ToString(), Id = Convert.ToInt32(reader["CourseId"]) };
                    student.State = new States() { Name = reader["StateName"].ToString(), Id = Convert.ToInt32(reader["StateId"]) };
                }
            }

            return student;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            List<Student> students = [];

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using SqlCommand command = new(QueryConstants.GetAllStudent, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Student student = new()
                    {
                        StudentId = (Guid)reader["StudentId"],
                        EnrollmentNo = reader["EnrollmentNo"].ToString()!,
                        FirstName = reader["FirstName"].ToString()!,
                        LastName = reader["LastName"].ToString()!,
                        DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                        Email = reader["Email"].ToString()!,
                        StateId = Convert.ToInt32(reader["StateId"]),
                        CourseId = Convert.ToInt32(reader["CourseId"]),
                        Course = new Course() { Name = reader["CourseName"].ToString(), Id = Convert.ToInt32(reader["CourseId"]) },
                        State = new States() { Name = reader["StateName"].ToString(), Id = Convert.ToInt32(reader["StateId"]) }
                    };
                    students.Add(student);
                }
            }
            return students;
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            var student = new Student();

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using SqlCommand command = new(QueryConstants.GetStudentByEnrollmentNo, connection);
                command.Parameters.AddWithValue("@EnrollmentNo", enrollmentNo);

                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {

                    student.StudentId = (Guid)reader["StudentId"];
                    student.EnrollmentNo = reader["EnrollmentNo"].ToString()!;
                    student.FirstName = reader["FirstName"].ToString()!;
                    student.LastName = reader["LastName"].ToString()!;
                    student.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null;
                    student.Email = reader["Email"].ToString()!;
                    student.StateId = Convert.ToInt32(reader["StateId"]);
                    student.CourseId = Convert.ToInt32(reader["CourseId"]);
                    student.Course = new Course() { Name = reader["CourseName"].ToString(), Id = Convert.ToInt32(reader["CourseId"]) };
                    student.State = new States() { Name = reader["StateName"].ToString(), Id = Convert.ToInt32(reader["StateId"]) };
                }
            }

            return student;
        }

        public async Task<List<KeyValuePair<string, int>>> GetChartDetails()
        {
            List<KeyValuePair<string, int>> chartDetails = [];

            using (var connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                using var command = new SqlCommand(QueryConstants.StudentChartDetails, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string stateName = reader["StateName"].ToString()!;
                    int count = Convert.ToInt32(reader["Count"]);
                    chartDetails.Add(new KeyValuePair<string, int>(stateName, count));
                }
            }

            return chartDetails;
        }

        public async Task<Student> Update(Student entity, string id)
        {
            using SqlConnection connection = _connection.CreateConnection();
            await connection.OpenAsync();

            using SqlCommand command = new(QueryConstants.UpdateStudent, connection);
            command.Parameters.AddWithValue("@FirstName", entity.FirstName);
            command.Parameters.AddWithValue("@LastName", entity.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@StateId", entity.StateId);
            command.Parameters.AddWithValue("@CourseId", entity.CourseId);
            command.Parameters.AddWithValue("@Active", entity.Active);
            command.Parameters.AddWithValue("@StudentId", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return entity;
        }

        private static string GenerateEnrollmentNumber()
        {

            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
