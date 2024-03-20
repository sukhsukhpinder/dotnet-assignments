using SchoolWebAppAPI.Models;

namespace SchoolWebAppAPI.Interfaces
{
    /// <summary>
    /// Interface methods for Registrations
    /// </summary>
    public interface IRegistrationData
    {
        /// <summary>
        /// To get states
        /// </summary>
        /// <returns></returns>
        Task<IList<States>> GetStates();

        /// <summary>
        /// To Save Registration
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
    
        Task<ResponseModel<string>> SaveRegistration(Registration obj);
        /// <summary>
        /// Get Registrations list
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<IList<Registration>> GetRegistrations(int UserId);

        /// <summary>
        /// To Get Registration Detail
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        Task<Registration> GetRegistrationDetail(int UserId,int RegId);

        /// <summary>
        /// To Delete Registration
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> DeleteRegistration(int UserId,int RegId);

        /// <summary>
        /// Get Dashboard Data
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<DashboardData> GetDashboardData(int UserId);
    }
}
