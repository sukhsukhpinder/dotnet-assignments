using Microsoft.Extensions.Caching.Memory;
using sf_dotenet_mod.Application.Services.Base;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IMemoryCache _cache;
        private readonly IRepositoryFactory _repositoryFactory;

        public CommonService(IRepositoryFactory repositoryFactory, IMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _cache = memoryCache;
            _commonRepository = _repositoryFactory.GetCommonRepository();

        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            string cacheKey = "ActiveStates";
            if (!_cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> states))
            {
                states = await _commonRepository.GetAllActiveStates();
                _cache.Set(cacheKey, states, TimeSpan.FromMinutes(10));
            }
            return states;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            string cacheKey = "ActiveCourses";
            if (!_cache.TryGetValue(cacheKey, out List<KeyValuePair<int, string>> courses))
            {
                courses = await _commonRepository.GetAllActiveCourse();
                _cache.Set(cacheKey, courses, TimeSpan.FromMinutes(10));
            }
            return courses;
        }
    }
}
