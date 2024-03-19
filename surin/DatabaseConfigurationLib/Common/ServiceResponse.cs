using System.Net;

namespace DatabaseConfigurationLib.Common
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorDetails { get; set; }
        public string? CorrelationId { get; set; }
        public T? Result { get; set; }
    }
}
