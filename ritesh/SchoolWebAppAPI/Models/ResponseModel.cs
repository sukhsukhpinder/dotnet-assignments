using System.ComponentModel.DataAnnotations;

namespace SchoolWebAppAPI.Models
{
    /// <summary>
    /// Common response model for all API's
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T>
    {
        public bool IsSuccessful { get; set; }
        public int? StatusCode { get; set; }

        public int? Id { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    /// <summary>
    /// ResponseMessages for returning in API 
    /// </summary>
    public static class ResponseMessages
    {
        public const string ValidationError = "Validations Errors";
        public const string GetData = "Data retreived successfully";
        public const string RecordDeleted = "Record deleted successfully";
        public const string LoggedIn = "User logged in successfully";
        public const string UserNotFound = "User not Found";
        public const string PasswordIncorrect = "Password is Incorrect";
        public const string RegistrationSubmitted = "Registration Submitted !";
        public const string SignupSucess = "Student signed up successfully !";
        public const string SomeErrorOccured = "Some Error Occured !";
        public const string UserAlreadyExists = "Email or Username already exists !";
        public const string StudentAdded = "Student Added Successfully..!";
    }
    /// <summary>
    /// ResponseMessages for returning in API 
    /// </summary>
    public static class ResponseCodes
    {
        public const int BadRequest = 400;
        public const int InternalServerError = 500;
        public const int NotFound = 404;

    }

    /// <summary>
    /// DTO for Refresh Token
    /// </summary>
    public class TokenApiDto
    {
        public int code { get; set; }
        public int? userId { get; set; }
        public string message { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class DashboardData
    {
        public List<StateData> StateData { get; set; }
        public List<UserRegisteredData> UserRegisteredData { get; set; }


    }
    public class Tokens
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

    }
    public class StateData
    {
        public int? UserCount { get; set; }
        public string StateName { get; set; }
    }
    public class UserRegisteredData
    {
        public bool AdmissionTaken { get; set; }
        public int? UserCount { get; set; }
    }
    public class LoginUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
