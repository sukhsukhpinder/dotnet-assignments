using EnrollHub.Application.Dtos.Model;
using EnrollHub.Application.Dtos.Request;
using EnrollHub.Application.Dtos.Response;
using EnrollHub.Application.Mappers;
using EnrollHub.Application.Services.Base;
using EnrollHub.Domain.Repositories;
using System.Security.Claims;

namespace EnrollHub.Application.Services
{
    /// <summary>
    /// Service class providing operations related to user management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="model">The registration model of the user.</param>
        /// <returns>A service response containing information about the added user.</returns>

        public async Task<ServiceResponse<UserResponse>> AddUser(RegisterModel model)
        {
           var user=Mapper.GetUserEntity(model);
           var newUser =await _usersRepository.AddUser(user, model.Password);
           var userResponse= Mapper.GetUserResponse(newUser);

            var response = new ServiceResponse<UserResponse>
            {
                IsSuccessful = !string.IsNullOrEmpty(userResponse.UserID),
                ErrorMessage = !string.IsNullOrEmpty(userResponse.UserID) ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = userResponse,
            };

            return response;
        }
        /// <summary>
        /// Adds user claims.
        /// </summary>
        /// <param name="claims">The claims to add for the user.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A service response indicating whether the operation was successful.</returns>

        public async Task<ServiceResponse<bool>> AddUserClaim(List<Claim> claims, string userId)
        {
            var isClaimAdded=await _usersRepository.AddUserClaim(claims, userId);
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isClaimAdded,
                ErrorMessage = isClaimAdded ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = isClaimAdded,
            };
            return response;
        }
        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="assignRole">The role assignment information.</param>
        /// <returns>A service response indicating whether the operation was successful.</returns>

        public async Task<ServiceResponse<bool>> AssignRole(AssignRole assignRole)
        {
            var isRoleAssigned= await _usersRepository.AssignRole(assignRole.UserId, assignRole.RoleName);
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isRoleAssigned,
                ErrorMessage = isRoleAssigned ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = isRoleAssigned,
            };
            return response;
        }

        /// <summary>
        /// Retrieves user claims by email.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A service response containing the user's claims.</returns>

        public async Task<ServiceResponse<IEnumerable<Claim>>> GetUserClaims(string email)
        {
            var claims= await _usersRepository.GetUserClaims(email);
            var response = new ServiceResponse<IEnumerable<Claim>>
            {
                IsSuccessful = claims.Any(),
                ErrorMessage = claims.Any() ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = claims,
            };
            return response;
        }
        /// <summary>
        /// Authenticates user login.
        /// </summary>
        /// <param name="model">The login model of the user.</param>
        /// <returns>A service response indicating whether the login was successful.</returns>

        public async Task<ServiceResponse<bool>> Login(LoginModel model)
        {
            var isValidUser= await _usersRepository.Login(model.Email, model.Password, model.RememberMe);
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isValidUser,
                ErrorMessage = isValidUser ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = isValidUser,
            };
            return response;
        }
        /// <summary>
        /// Signs out the user.
        /// </summary>
        /// <returns>A service response indicating whether the sign-out was successful.</returns>

        public async Task<ServiceResponse<bool>> SignOut()
        {
            var isSingOutSuccessfully= await _usersRepository.SignOut();
            var response = new ServiceResponse<bool>
            {
                IsSuccessful = isSingOutSuccessfully,
                ErrorMessage = isSingOutSuccessfully ? Constants.OperationSuccessful : Constants.OperationFailed,
                Result = isSingOutSuccessfully,
            };
            return response;
        }
    }
}
