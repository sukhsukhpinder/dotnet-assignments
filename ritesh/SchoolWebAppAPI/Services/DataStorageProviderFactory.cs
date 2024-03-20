using SchoolWebAppAPI.Context;
using SchoolWebAppAPI.Interfaces;
using SchoolWebAppAPI.Services;
using StudentRegistrationApi.Interfaces;

namespace SchoolWebAppAPI.Classes
{
    public class DataStorageProviderFactory 
    {
        private readonly AppDbContext _authContext;
        public DataStorageProviderFactory(AppDbContext context)
        {
            _authContext = context;
        }
        /// <summary>
        /// This methods returns whether to use sql as db or file depending upon useSQL flag in appsetting.json
        /// </summary>
        /// <param name="useSql"></param>
        /// <returns></returns>
        public IRegistrationData GetProvider(bool useSql)
        {
            return useSql ? new RegistrationDataSQL(_authContext) : new RegistrationDataFile();
        }
        public IStudentData GetProviderStudent(bool useSql)
        {
            return useSql ? new StudentDataSQL(_authContext) : new StudentDataFile();
        }

    }
 
 
}
