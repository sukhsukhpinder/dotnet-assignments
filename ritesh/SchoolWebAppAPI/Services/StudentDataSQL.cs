using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebAppAPI.Context;
using SchoolWebAppAPI.Helpers;
using SchoolWebAppAPI.Models;
using StudentRegistrationApi.Interfaces;

namespace SchoolWebAppAPI.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentDataSQL : IStudentData
    {
        private readonly AppDbContext _authContext;
        public StudentDataSQL(AppDbContext context)
        {
            _authContext = context;
        }


        async Task<ResponseModel<Tokens>> IStudentData.RefreshToken(TokenApiDto tokenApiDto)
        {

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = PasswordHasher.GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            var newAccessToken = PasswordHasher.CreateJwt(user);
            var newRefreshToken = PasswordHasher.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _authContext.SaveChangesAsync();
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
        async Task<ResponseModel<Tokens>> IStudentData.StudentLogin(LoginUser userObj)
        {


            var user = await _authContext.Users
                .FirstOrDefaultAsync(x => x.Username == userObj.Username);

            if (user == null)
                return (new ResponseModel<Tokens>
                { IsSuccessful = false, Message = ResponseMessages.UserNotFound, StatusCode = StatusCodes.Status400BadRequest });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return (new ResponseModel<Tokens>
                { IsSuccessful = false, Message = ResponseMessages.PasswordIncorrect, StatusCode = StatusCodes.Status400BadRequest });
            }

            user.Token = PasswordHasher.CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = PasswordHasher.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _authContext.SaveChangesAsync();
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
       /// <summary>
       /// Add student to SQL db
       /// </summary>
       /// <param name="userObj"></param>
       /// <returns></returns>
        async Task<ResponseModel<string>> IStudentData.AddStudent(User userObj)
        {
            // check email
            if (await CheckEmailExistAsync(userObj.Email, userObj.Username))
                return (new ResponseModel<string>
                {
                    IsSuccessful = false
                    ,
                    Message = ResponseMessages.UserAlreadyExists,
                    StatusCode = StatusCodes.Status400BadRequest
                });


            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _authContext.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.StudentAdded,
                StatusCode = StatusCodes.Status200OK
            });
        }
        private Task<bool> CheckEmailExistAsync(string email, string username)
         => _authContext.Users.AnyAsync(x => x.Email == email || x.Username == username);


    }
}
