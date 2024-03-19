namespace Database.Contracts
{
    public interface ICommonContract
    {
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();
    }
}
