using Database.Entities;
using Registration.API.Dtos;

namespace Registration.API.Mapper
{
    public static class Mapper
    {
        #region StudentMapper
        public static Student GetStudentEntity(StudentRequest request)
        {
            Student entity = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                CourseId = request.CourseId,
                StateId = request.StateId,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                isActive = request.IsActive
            };

            return entity;

        }

        public static StudentResponse GetStudentResponse(Student entity)
        {
            StudentResponse response = new StudentResponse
            {
                StudentId = entity.StudentId.ToString(),
                EnrollmentNo = entity.EnrollmentNo,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FullName = $"{entity.FirstName} {entity.LastName}",
                CourseId = entity.CourseId,
                StateId = entity.StateId,
                DateOfBirth = entity.DateOfBirth,
                Email = entity.Email,
                IsActive = entity.isActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn

            };
            return response;
        }
        #endregion
    }
}
