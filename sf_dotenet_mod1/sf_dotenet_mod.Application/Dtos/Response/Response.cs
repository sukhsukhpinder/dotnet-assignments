namespace sf_dotenet_mod.Application.Dtos.Response
{
    public class Response<T>
    {
        public string Status { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorDetails { get; set; }
        public string CorrelationId { get; set; }
        public T Result { get; set; }
    }
}
