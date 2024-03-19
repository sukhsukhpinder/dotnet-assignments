namespace Registration.API.Services.Interface
{
    public interface ICommonService
    {
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();
    }
}
