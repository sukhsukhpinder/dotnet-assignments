using Microsoft.EntityFrameworkCore;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class CommonRepository(EnrollDbContext dbContext) : ICommonRepository
    {
        private readonly EnrollDbContext _dbContext = dbContext;

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            var courses = await _dbContext.Courses
                .Where(c => c.Active)
                .Select(c => new KeyValuePair<int, string>(c.Id, c.Name))
                .ToListAsync();

            return courses;
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            var states = await _dbContext.States
                .Where(s => s.Active)
                .Select(s => new KeyValuePair<int, string>(s.Id, s.Name))
                .ToListAsync();

            return states;
        }
    }
}
