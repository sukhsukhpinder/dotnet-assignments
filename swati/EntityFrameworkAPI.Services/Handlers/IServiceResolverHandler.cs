using EntityFrameworkAPI.Services.Services.Interface;

namespace EntityFrameworkAPI.Services.Handlers
{
    public interface IServiceResolverHandler
    {
        IStudentService GetServiceResolver();
    }
}
