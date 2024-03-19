namespace sf_dotenet_mod.Domain.Repositories
{
    /// <summary>
    /// Interface for a repository that provides common data retrieval operations.
    /// </summary>
    public interface ICommonRepository
    {
        /// <summary>
        /// Retrieves a list of all active courses.
        /// </summary>
        /// <returns>A list of key-value pairs representing active courses.</returns>
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();

        /// <summary>
        /// Retrieves a list of all active states.
        /// </summary>
        /// <returns>A list of key-value pairs representing active states.</returns>
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();
    }
}
