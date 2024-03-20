using AuthenticationManager.Data;
using EntityFrameworkAPI.Core.UnitOfWork;
using EntityFrameworkAPI.Services.Services;
using EntityFrameworkAPI.Services.Services.Interface;

namespace EntityFrameworkAPI.Services.Handlers
{
    public class ServiceResolverHandler : IServiceResolverHandler
    {
        private readonly IUnit _unitOfWork;
        private readonly IXMLUnit _xmlUnitOfWork;
        private readonly IDapperUnit _dapperUnitOfWork;
        public ServiceResolverHandler(IUnit unitOfWork, IXMLUnit xmlUnitOfWork, IDapperUnit dapperUnitOfWork)
        {
            _unitOfWork = unitOfWork;
            _xmlUnitOfWork = xmlUnitOfWork;
            _dapperUnitOfWork = dapperUnitOfWork;
        }

        public IStudentService GetServiceResolver()
        {
            var config = ConfigSettings.StorageProvider;

            switch (config)
            {
                case "SqlServer":
                    return new StudentService(_unitOfWork);
                case "XML":
                    return new XMLService(_xmlUnitOfWork);
                case "Dapper":
                    return new DapperService(_dapperUnitOfWork);
                case "AdoDotNet":
                    return null; //new AdoService(_adoUnitOfWork);
                default:
                    return null;
            }
        }
    }
}
