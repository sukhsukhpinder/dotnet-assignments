using Microsoft.EntityFrameworkCore;
using SchoolWebAppAPI.Context;
using SchoolWebAppAPI.Interfaces;
using SchoolWebAppAPI.Models;

namespace SchoolWebAppAPI.Classes
{
    public class RegistrationDataSQL : IRegistrationData
    {
        private readonly AppDbContext _authContext;
        public RegistrationDataSQL(AppDbContext context)
        {
            _authContext = context;
        }
        public async Task<IList<States>> GetStates()
        {
            return await _authContext.States.ToListAsync();
        }
        /// <summary>
        /// Used to Save Registration
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> SaveRegistration(Registration obj)
        {

            var existingObj = await _authContext.Registration.FindAsync(obj.Id);

            if (existingObj != null)
            {
                obj.IsActive = existingObj.IsActive;
                obj.CreatedOn = existingObj.CreatedOn;
                obj.UpdatedOn = DateTime.UtcNow;
                // Update existing object
                _authContext.Entry(existingObj).CurrentValues.SetValues(obj);
            }
            else
            {
                obj.IsActive = true;
                obj.CreatedOn = DateTime.UtcNow;
                // Add new object
                await _authContext.AddAsync(obj);
            }
            await _authContext.SaveChangesAsync();
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.RegistrationSubmitted,
                StatusCode = StatusCodes.Status200OK
            });

        }
        /// <summary>
        /// To DeleteRegistration done by user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> DeleteRegistration(int UserId, int RegId)
        {

            var existingObj = await _authContext.Registration.FindAsync(RegId);

            if (existingObj != null)
            {
                existingObj.IsActive = false;
            }

            await _authContext.SaveChangesAsync();
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.RecordDeleted,
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// To get submitted Registrations
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IList<Registration>> GetRegistrations(int UserId)
        {
            return await _authContext.Registration.Where(x => x.UserId == UserId && x.IsActive == true).Include(x => x.State).ToListAsync();

        }
        /// <summary>
        /// To Get RegistrationDetails
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegId"></param>
        /// <returns></returns>
        public async Task<Registration> GetRegistrationDetail(int UserId, int RegId)
        {
            return await _authContext.Registration.Where(x => x.Id == RegId && x.UserId == UserId).FirstOrDefaultAsync();

        }
        /// <summary>
        /// To Get DashboardData
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<DashboardData> GetDashboardData(int UserId)
        {
            DashboardData obj = new DashboardData();

            obj.StateData = await _authContext.Registration
             .Join(_authContext.States, reg => reg.StateId, state => state.Id, (reg, state) => new { reg.UserId, state.StateName })
            .GroupBy(r => r.StateName)
            .Select(g => new StateData { StateName = g.Key, UserCount = g.Count() }).ToListAsync();
            obj.UserRegisteredData = await _authContext.Registration
            .GroupBy(r => r.AdmissionTaken)
            .Select(g => new UserRegisteredData { AdmissionTaken = g.Key, UserCount = g.Count() }).ToListAsync();
            return obj;

        }


    }
}
