using EnrollHub.Application.Dtos.Request;

namespace EnrollHub.Application.Dtos.Response
{
    public class UserResponse: UserRequest
    {
        public string UserID { get; set; }
    }
}
