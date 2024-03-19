namespace sf_dotenet_mod.Domain.Repositories
{
    /// <summary>
    /// Interface for a factory that creates repository instances.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets an instance of the common repository.
        /// </summary>
        /// <returns>An instance of the common repository.</returns>
        ICommonRepository GetCommonRepository();

        /// <summary>
        /// Gets an instance of the student repository.
        /// </summary>
        /// <returns>An instance of the student repository.</returns>
        IStudentRepository GetStudentRepository();
    }
}
