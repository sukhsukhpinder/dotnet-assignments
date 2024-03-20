using Newtonsoft.Json;
using SchoolWebAppAPI.Interfaces;
using SchoolWebAppAPI.Models;

namespace SchoolWebAppAPI.Classes
{
    /// <summary>
    /// Methods for Registration
    /// </summary>
    public class RegistrationDataFile : IRegistrationData
    {
        private string _path = "User.json";
        private readonly object _lockObject = new object();

        public async Task<ResponseModel<string>> DeleteRegistration(int UserId, int RegId)
        {
            List<Registration> users = await GetAllUsersAsync();

            int index = users.FindIndex(u => u.Id == RegId && u.UserId == UserId);
            if (index != -1)
            {
                users[index].IsActive = false;
            }
            SaveChanges(users);
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.RecordDeleted,
                StatusCode = StatusCodes.Status200OK
            });
        }

        async Task<DashboardData> IRegistrationData.GetDashboardData(int UserId)
        {
            DashboardData obj = new DashboardData();
            var users = await GetAllUsersAsync();
            List<StateData> stateList = new List<StateData>();
            stateList.Add(new StateData { StateName = "All", UserCount = users.Count() });
            obj.StateData = stateList;
            obj.UserRegisteredData = users
            .GroupBy(r => r.AdmissionTaken)
            .Select(g => new UserRegisteredData { AdmissionTaken = g.Key, UserCount = g.Count() }).ToList();
            return obj;
        }

        async Task<Registration> IRegistrationData.GetRegistrationDetail(int UserId, int RegId)
        {

            var users = await GetAllUsersAsync();
            return users.Where(x => x.Id == RegId && x.UserId == UserId).FirstOrDefault();
        }

        async Task<IList<Registration>> IRegistrationData.GetRegistrations(int UserId)
        {
            return await GetAllUsersAsync();
        }
        /// <summary>
        /// To get all users
        /// </summary>
        /// <returns>list </returns>
        public async Task<List<Registration>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (!File.Exists(_path))
                        return new List<Registration>();

                    string jsonData = File.ReadAllText(_path);
                    return JsonConvert.DeserializeObject<List<Registration>>(jsonData).Where(x => x.IsActive == true).ToList();
                }
            });
        }
        private void SaveChanges(List<Registration> users)
        {
            lock (_lockObject)
            {
                string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllTextAsync(_path, jsonData);
            }
        }
        Task<IList<States>> IRegistrationData.GetStates()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> SaveRegistration(Registration obj)
        {
            List<Registration> users = await GetAllUsersAsync();
            if (obj.Id > 0)
            {
                int index = users.FindIndex(u => u.Id == obj.Id);
                if (index != -1)
                {
                    obj.IsActive = users[index].IsActive;
                    obj.CreatedOn = users[index].CreatedOn;
                    obj.UpdatedOn = DateTime.UtcNow;
                    users[index] = obj;
                }
            }
            else
            {
                obj.IsActive = true;
                obj.CreatedOn = DateTime.UtcNow;
                obj.Id = users.Count() + 1;
                users.Add(obj);
            }
            SaveChanges(users);
            return (new ResponseModel<string>
            {
                IsSuccessful = true,
                Message = ResponseMessages.RegistrationSubmitted,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
