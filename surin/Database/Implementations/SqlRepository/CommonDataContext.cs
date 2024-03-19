using Database.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Database.Implementations.SqlRepository
{
    public class CommonDataContext : ICommonContract
    {
        private readonly RegistrationDBContext context;

        public CommonDataContext(RegistrationDBContext context)
        {
            this.context = context;
        }
        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            var courses = await context.Courses
                .Where(c => c.isActive)
                .Select(c => new KeyValuePair<int, string>(c.Id, c.Name))
                .ToListAsync();

            return courses;
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            var states = await context.States
                 .Where(s => s.isActive)
                 .Select(s => new KeyValuePair<int, string>(s.Id, s.Name))
                 .ToListAsync();

            return states;
        }
    }
}
