using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sf_dotenet_mod.Infrastructure
{
    public class SecondaryRepository : IRepository1
    {
        private readonly SecondaryDbContext _secondaryDbContext;

        public SecondaryRepository(SecondaryDbContext secondaryDbContext)
        {
            _secondaryDbContext = secondaryDbContext;
        }

        public async Task<bool> TestConnection()
        {
            return await _secondaryDbContext.Database.CanConnectAsync();
        }
    }
}
