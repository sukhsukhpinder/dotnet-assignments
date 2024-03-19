using Database.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Registration.API.Services.Interface;

namespace Registration.API.Services.Implementations
{
    public class CommonService : ICommonService
    {
        private readonly ICommonContract contract;
        private readonly IMemoryCache cache;

        public CommonService(ICommonContract contract, IMemoryCache memoryCache)
        {
            this.contract = contract;
            this.cache = memoryCache;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            string cacheKey = "ActiveCourses";
            if (!cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> courses))
            {
                courses = await contract.GetAllActiveCourse();
                cache.Set(cacheKey, courses, TimeSpan.FromMinutes(10));
            }
            return courses!;
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            string cacheKey = "ActiveStates";
            if (!cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> states))
            {
                states = await contract.GetAllActiveStates();
                cache.Set(cacheKey, states, TimeSpan.FromMinutes(10));
            }
            return states!;
        }
    }
}
