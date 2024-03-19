using Auth.API.Constants;
using Auth.API.Dto;
using Auth.API.Services.Interface;
using Database.Contracts;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Dto;
using System.Net;

namespace Auth.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserContract _contract;
        private readonly JwtTokenHandler _jwtTokenHandler;

        public UserService(IUserContract contract, JwtTokenHandler jwtTokenHandler)
        {
            _contract = contract;
            _jwtTokenHandler = jwtTokenHandler;
        }

        public async Task<ServiceResponse<bool>> RegisterUser(RegistrationRequest registrationRequest)
        {
            var serviceResponse = new ServiceResponse<bool>();            
            var user = Mapper.Mapper.GetUserEntity(registrationRequest);
            var data = await _contract.RegisterUser(user);
            if (data)
            {
                serviceResponse.IsSuccessful = true;
                serviceResponse.Result = data;
                serviceResponse.Status = HttpStatusCode.Created;
            }
            else
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.ErrorDetails = ResponseConstants.FailedRegistration;
                serviceResponse.Status = HttpStatusCode.InternalServerError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthenticationResponse>> AuthenticateUser(AuthenticationRequest authenticationRequest)
        {
            var authenticateResponse = await _jwtTokenHandler.GenerateJwtToken(authenticationRequest);

            var response = new ServiceResponse<AuthenticationResponse>();
            if (authenticateResponse != null)
            {
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.OK;
                response.Result = authenticateResponse;
            }
            else
            {
                response.IsSuccessful = false;
                response.Status = HttpStatusCode.Unauthorized;
                response.ErrorDetails = ResponseConstants.LoginFailed;
            }

            return response;

        }
    }
}
