namespace sf_dotenet_mod.Infrastructure
{
    public static class QueryConstants
    {
        public const string InsertStudent = "INSERT INTO Students (StudentId, FirstName, LastName, DateOfBirth,Email,StateId,CourseId, EnrollmentNo, Active) VALUES (@StudentId,@FirstName, @LastName, @DateOfBirth,@Email,@StateId,@CourseId, @EnrollmentNo, @Active);";

        public const string DeleteStudent = "DELETE FROM Students WHERE StudentId = @StudentId";

        public const string DeleteStudentByEnrollment = "DELETE FROM Students WHERE EnrollmentNo = @EnrollmentNo";

        public const string GetStudentById = @"SELECT s.StudentId, s.EnrollmentNo, s.FirstName, s.LastName, s.DateOfBirth, s.Email, s.StateId, s.CourseId, s.Active, c.Name AS CourseName, st.Name AS StateName
                                               FROM Students s INNER JOIN Courses c ON s.CourseId = c.Id INNER JOIN States st ON s.StateId = st.Id WHERE s.StudentId = @StudentId";

        public const string GetAllStudent = @"SELECT s.StudentId, s.EnrollmentNo, s.FirstName, s.LastName, s.DateOfBirth, s.Email, s.StateId, s.CourseId, s.Active, c.Name AS CourseName, st.Name AS StateName
                                               FROM Students s INNER JOIN Courses c ON s.CourseId = c.Id INNER JOIN States st ON s.StateId = st.Id";

        public const string GetStudentByEnrollmentNo = @"SELECT s.StudentId, s.EnrollmentNo, s.FirstName, s.LastName, s.DateOfBirth, 
                                                       s.Email, s.StateId, s.CourseId, s.Active, c.Name AS CourseName, st.Name AS StateName
                                                       FROM Students s
                                                       INNER JOIN Courses c ON s.CourseId = c.Id
                                                       INNER JOIN States st ON s.StateId = st.Id
                                                       WHERE s.EnrollmentNo = @EnrollmentNo";

        public const string StudentChartDetails = @"SELECT s1.[Name] AS StateName, COUNT(*) AS Count FROM Students s JOIN States s1 ON s.StateId = s1.Id GROUP BY s1.[Name]";

        public const string UpdateStudent = @"UPDATE Students SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Email = @Email, StateId = @StateId,
                                              CourseId = @CourseId, Active = @Active WHERE StudentId = @StudentId";
    }
}
