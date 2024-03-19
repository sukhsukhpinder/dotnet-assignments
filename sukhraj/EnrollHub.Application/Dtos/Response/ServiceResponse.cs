namespace EnrollHub.Application.Dtos.Response
{
    /// <summary>
    /// Represents a generic service response containing information about the success of an operation,
    /// any error messages encountered, and the resulting data.
    /// </summary>
    /// <typeparam name="T">Type of the result data.</typeparam>
    public class ServiceResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}
