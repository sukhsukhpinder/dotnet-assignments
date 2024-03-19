using EnrollHub.Application.Dtos.Model;
using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Dtos.Response;
using System.Security.Claims;

namespace EnrollHub.Application.Services.Base
{
    public interface IUserService
    {
        Task<ServiceResponse<UserResponse>> AddUser(RegisterModel model);
        Task<ServiceResponse<bool>> AssignRole(AssignRole assignRole);
        Task<ServiceResponse<bool>> Login(LoginModel model);
        Task<ServiceResponse<IEnumerable<Claim>>> GetUserClaims(string email);
        Task<ServiceResponse<bool>> AddUserClaim(List<Claim> claims, string userId);
        Task<ServiceResponse<bool>> SignOut();
    }
}
