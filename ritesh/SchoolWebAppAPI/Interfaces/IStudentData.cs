using Microsoft.AspNetCore.Mvc;
using SchoolWebAppAPI.Models;

namespace StudentRegistrationApi.Interfaces
{
    /// <summary>
    /// Student Related Methods
    /// </summary>
    public interface IStudentData
    {
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userObj"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddStudent(User userObj);

        /// <summary>
        /// Login student
        /// </summary>
        /// <param name="userObj"></param>
        /// <returns></returns>
        Task<ResponseModel<Tokens>> StudentLogin(LoginUser userObj);

        /// <summary>
        /// To refresh user token
        /// </summary>
        /// <param name="tokenApiDto"></param>
        /// <returns></returns>
        Task<ResponseModel<Tokens>> RefreshToken(TokenApiDto tokenApiDto);
 
    }
}
