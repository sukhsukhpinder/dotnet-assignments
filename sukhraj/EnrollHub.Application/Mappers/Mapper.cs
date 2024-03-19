using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Dtos.Response;
using EnrollHub.Domain.Entities;

namespace EnrollHub.Application.Mappers
{
    public static class Mapper
    {
        #region StudentMapper
        public static Student GetStudentEntity(StudentRequest request)
        {
            Student entity = new Student
            {
                FirstName= request.FirstName,
                LastName= request.LastName,
                CourseId= request.CourseId,
                StateId= request.StateId,   
                DateOfBirth=request.DateOfBirth,
                Email= request.Email
            };

            return entity;

        }

        public static StudentResponse GetStudentResponse(Student entity)
        {
            if(entity==null)
            {
                return null;
            }
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
                Active=entity.Active,
                CreatedBy=entity.CreatedBy,
                CreatedOn=entity.CreatedOn,
                ModifiedBy=entity.ModifiedBy,
                ModifiedOn=entity.ModifiedOn,

                StateName=entity.State?.Name,
                CourseName=entity.Course?.Name

            };
            return response;
        }
        #endregion

        #region UserMapper
        public static User GetUserEntity(RegisterModel model)
        {
            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber=model.PhoneNumber,
                UserName=model.Email
            };

            return user;
        }

        public static User GetUserEntity(UserRequest request)
        {
            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber=request.PhoneNumber,
                Email = request.Email
            };

            return user;
        }

        public static UserResponse GetUserResponse(User entity)
        {
            UserResponse response = new UserResponse
            {
                UserID = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email
            };

            return response;
        }
        #endregion
    }
}
