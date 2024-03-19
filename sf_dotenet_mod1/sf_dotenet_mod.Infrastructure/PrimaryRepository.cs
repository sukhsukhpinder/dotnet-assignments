using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sf_dotenet_mod.Infrastructure
{
    public class PrimaryRepository : IRepository1
    {
        private readonly PrimaryDbContext _primaryDbContext;
        public PrimaryRepository(PrimaryDbContext primaryDbContext)
        {
            _primaryDbContext = primaryDbContext;
        }
        public async Task<bool> TestConnection()
        {
            return await _primaryDbContext.Database.CanConnectAsync();
        }
    }

    public interface IRepository1
    {
        Task<bool> TestConnection();
    }
}
