namespace sf_dotenet_mod.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly PrimaryDbContext _primaryDbContext;
        private readonly SecondaryDbContext _secondaryDbContext;

        public Repository(PrimaryDbContext primaryDbContext, SecondaryDbContext secondaryDbContext)
        {
            _primaryDbContext = primaryDbContext;
            _secondaryDbContext = secondaryDbContext;
        }

        public async Task<bool> TestConnection(DataSource dataSource)
        {
            return dataSource switch
            {
                DataSource.primary => await _primaryDbContext.Database.CanConnectAsync(),
                DataSource.secondary => await _secondaryDbContext.Database.CanConnectAsync(),
                _ => throw new ArgumentOutOfRangeException(nameof(dataSource), dataSource, null)
            };
        }
    }

    public interface IRepository
    {
        Task<bool> TestConnection(DataSource dataSource);
    }
}
