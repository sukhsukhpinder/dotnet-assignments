using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SchoolWebAppAPI.Helpers;
using SchoolWebAppAPI.Models;
using StudentRegistrationApi.Interfaces;

namespace SchoolWebAppAPI.Services
{
    public class StudentDataFile : IStudentData
    {
        private string _path = "Students.json";
        private readonly object _lockObject = new object();
        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (!File.Exists(_path))
                        return new List<User>();

                    string jsonData = File.ReadAllText(_path);
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        return JsonConvert.DeserializeObject<List<User>>(jsonData).Where(x => x.IsActive == true).ToList();

                    }
                    else
                    {
                        return new List<User>();
                    }
                }
            });
        }
        private void SaveChanges(List<User> users)
        {
            lock (_lockObject)
            {
                string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllTextAsync(_path, jsonData);
            }
        }
        public async Task<ResponseModel<string>> AddStudent(User obj)
        {
            List<User> students = await GetAllStudentsAsync();
            obj.IsActive = true;
            obj.CreatedOn = DateTime.UtcNow;
            obj.Id = students.Count() + 1;
            obj.Password = PasswordHasher.HashPassword(obj.Password);
            obj.Role = "User";
            obj.Token = "";
            students.Add(obj);
            SaveChanges(students);
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.SignupSucess,
                StatusCode=StatusCodes.Status200OK

            });
        }

        public async Task<ResponseModel<Tokens>> RefreshToken(TokenApiDto tokenApiDto)
        {
            List<User> students = await GetAllStudentsAsync();

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = PasswordHasher.GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = students.FirstOrDefault(u => u.Username == username);
            int index = students.FindIndex(u => u.Username == username);

            var newAccessToken = PasswordHasher.CreateJwt(user);
            var newRefreshToken = PasswordHasher.CreateRefreshToken();
            students[index].RefreshToken = newRefreshToken;

            SaveChanges(students);
            Tokens obj = new Tokens
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return (new ResponseModel<Tokens>
            {
                IsSuccessful        = true,
                Message = ResponseMessages.LoggedIn,
                Data = obj,
                Id = user.Id,
                StatusCode = StatusCodes.Status200OK

            });
        }

        public async Task<ResponseModel<Tokens>> StudentLogin(LoginUser userObj)
        {
            List<User> students = await GetAllStudentsAsync();
            var user =  students.FirstOrDefault(x => x.Username == userObj.Username);
            if (user == null)
                return (new ResponseModel<Tokens>
                { IsSuccessful = false, Message = ResponseMessages.UserNotFound,
                    StatusCode = StatusCodes.Status400BadRequest
                });

          
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return (new ResponseModel<Tokens>
                { IsSuccessful = false, Message = ResponseMessages.PasswordIncorrect, StatusCode = StatusCodes.Status400BadRequest });
            }
            int index = students.FindIndex(u => u.Username == userObj.Username);

            students[index].Token = PasswordHasher.CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = PasswordHasher.CreateRefreshToken();
            students[index].RefreshToken = newRefreshToken;
            students[index].RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            SaveChanges(students);
            Tokens obj = new Tokens
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return (new ResponseModel<Tokens>
            {
                IsSuccessful = true,
                Message = ResponseMessages.LoggedIn,
                Data = obj,
                Id = user.Id,
                StatusCode = StatusCodes.Status200OK

            });
        }
    }
}
