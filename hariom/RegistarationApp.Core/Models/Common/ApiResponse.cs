namespace RegistarationApp.Core.Models.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public string? CorrelationId { get; set; }
    }
}
