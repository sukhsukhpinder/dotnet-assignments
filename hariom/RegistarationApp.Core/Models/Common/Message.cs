namespace RegistarationApp.Core.Models.Common
{
    public static class Message
    {
        public const string Deleted = "deleted";
        public const string Updated = "updated";
        public const string Created = "created";
        public const string LoginSucsess = "login sucsessfully";
        public const string LoginFailed = "login failed";
        public const string DataFound = "data found successfully";
        public const string DataNotFound = "data not found";
        public const string InvalidRefreshToken = "invalid refresh token";
        public const string InvalidFilePath = "File path cannot be null or empty for file system storage";
        public const string InvalidRepositorySetting = "Invalid Repository Setting";
        public const string InvalidStorageType = "Invalid storage type";
        public const string InvalidDataAccesslayer = "Invalid Data Access Layer";
        public const string InvalidEmailPassword = "Invalid Email or Password.";
        public const string CourseAlreadyExist = "Course already exist";
        public const string UserAlreadyExist = "User already exist";
    }
}
