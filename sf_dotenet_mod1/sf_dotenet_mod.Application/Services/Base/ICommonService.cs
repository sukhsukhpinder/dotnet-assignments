namespace sf_dotenet_mod.Application.Services.Base
{
    /// <summary>
    /// Interface for a service that provides common operations related to courses and states.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Retrieves a list of all active courses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of key-value pairs representing active courses.</returns>
        Task<List<KeyValuePair<int, string>>> GetAllActiveCourse();

        /// <summary>
        /// Retrieves a list of all active states.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of key-value pairs representing active states.</returns>
        Task<List<KeyValuePair<int, string>>> GetAllActiveStates();
    }
}
