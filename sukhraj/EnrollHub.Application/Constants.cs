using EnrollHub.Domain.Entities;

namespace EnrollHub.Application
{
    public static class Constants
    {
        public const string Role = "Role";
        public const string Admin = "Admin";
        public const string User = "User"; 
  


        #region ErrorMessage

        public const string OperationSuccessful = "Operation successful";
        public const string OperationFailed = "Operation failed";
        public const string UnexpectedError ="An unexpected error occurred";
        public const string UnexpectedErrorNoExceptionInformation = "An unexpected error occurred, but no exception information was available.";
        public const string StudentIdCannotBeNull="Student ID cannot be null or empty.";
        public const string MinimumEnrollmentAgeMessage = "Student should be at least 16 years old to enroll.";
        public const string DateOfBirthCannotBeInTheFutureMessage = "Date of birth cannot be greater than today's date.";




        #endregion


    }
}
