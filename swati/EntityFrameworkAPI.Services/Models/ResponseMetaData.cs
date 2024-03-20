using System.Net;

namespace EntityFrameworkAPI.Models
{
    public class ResponseMetaData<T>
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;
        public string Message { get; set; }
        public bool IsError { get; set; } = true;
        public string ErrorDetails { get; set; }
        public string CorrealtionId { get; set; }
        public T Result { get; set; }
    }
}
