using Database.Contracts;
using Database.Entities;
using Microsoft.Data.SqlClient;

namespace Database.Implementations.SqlRepository.Students
{
    public class StudentAdoRepository : IStudentContract
    {
        private readonly string _connectionString;

        public StudentAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private async Task<SqlConnection> OpenConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<bool> DeleteEnrollmentById(int id)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Students WHERE StudentId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Students WHERE EnrollmentNo = @EnrollmentNo", connection))
                {
                    command.Parameters.AddWithValue("@EnrollmentNo", enrollmentNo);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<Student> EnrollStudent(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.isActive = entity.isActive || true;

            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await EnrollStudent(entity, connection, transaction);
                        transaction.Commit();
                        return entity;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task<int> EnrollStudent(Student entity, SqlConnection connection, SqlTransaction transaction)
        {
            const string insertQuery = "INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, StateId, CourseId, EnrollmentNo, isActive) " +
                                       "VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @StateId, @CourseId, @EnrollmentNo, @IsActive); SELECT SCOPE_IDENTITY();";

            using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@StateId", entity.StateId);
                command.Parameters.AddWithValue("@CourseId", entity.CourseId);
                command.Parameters.AddWithValue("@EnrollmentNo", entity.EnrollmentNo);
                command.Parameters.AddWithValue("@IsActive", entity.isActive);

                object newStudentId = await command.ExecuteScalarAsync();
                return Convert.ToInt32(newStudentId);
            }
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Students", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Student student = new Student
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                EnrollmentNo = reader["EnrollmentNo"].ToString()!,
                                FirstName = reader["FirstName"].ToString()!,
                                LastName = reader["LastName"].ToString()!,
                                DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                Email = reader["Email"].ToString()!,
                                StateId = Convert.ToInt32(reader["StateId"]),
                                CourseId = Convert.ToInt32(reader["CourseId"])
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE EnrollmentNo = @EnrollmentNo", connection))
                {
                    command.Parameters.AddWithValue("@EnrollmentNo", enrollmentNo);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Student
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                EnrollmentNo = reader["EnrollmentNo"].ToString()!,
                                FirstName = reader["FirstName"].ToString()!,
                                LastName = reader["LastName"].ToString()!,
                                DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                Email = reader["Email"].ToString()!,
                                StateId = Convert.ToInt32(reader["StateId"]),
                                CourseId = Convert.ToInt32(reader["CourseId"])
                            };
                        }
                    }
                }
            }

            return null!;
        }

        public async Task<Student> GetById(int id)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE StudentId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Student
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                EnrollmentNo = reader["EnrollmentNo"].ToString()!,
                                FirstName = reader["FirstName"].ToString()!,
                                LastName = reader["LastName"].ToString()!,
                                DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                Email = reader["Email"].ToString()!,
                                StateId = Convert.ToInt32(reader["StateId"]),
                                CourseId = Convert.ToInt32(reader["CourseId"])
                            };
                        }
                    }
                }
            }

            return null!;
        }

        public async Task<Student> UpdateEnrollment(Student entity, int id)
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Email = @Email, StateId = @StateId, CourseId = @CourseId, EnrollmentNo = @EnrollmentNo, isActive = @IsActive WHERE StudentId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    command.Parameters.AddWithValue("@LastName", entity.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", entity.Email);
                    command.Parameters.AddWithValue("@StateId", entity.StateId);
                    command.Parameters.AddWithValue("@CourseId", entity.CourseId);
                    command.Parameters.AddWithValue("@EnrollmentNo", entity.EnrollmentNo);
                    command.Parameters.AddWithValue("@IsActive", entity.isActive);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return entity;
                    }
                }
            }

            return null!;
        }

        public async Task<IEnumerable<(string StateName, double Percentage)>> GetStateStudentPercentage()
        {
            List<(string StateName, double Percentage)> statePercentages = new List<(string, double)>();

            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT s.Name AS StateName, COUNT(st.StudentId) * 100.0 / (SELECT COUNT(*) FROM Students) AS Percentage FROM States s LEFT JOIN Students st ON s.Id = st.StateId GROUP BY s.Name", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statePercentages.Add((
                                reader["StateName"].ToString()!,
                                Convert.ToDouble(reader["Percentage"])
                            ));
                        }
                    }
                }
            }

            return statePercentages;
        }

        public async Task<double> GetSuccessfulJoinPercentage()
        {
            using (SqlConnection connection = await OpenConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Students) AS Percentage FROM Students WHERE isActive = 1", connection))
                {
                    object result = await command.ExecuteScalarAsync();
                    return result != DBNull.Value ? Convert.ToDouble(result) : 0;
                }
            }
        }

        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
