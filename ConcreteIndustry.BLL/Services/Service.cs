using AutoMapper;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class Service : IService
    {
        private readonly ILogger<Service> logger;
        private readonly ICacheService cacheService;

        public IAccountService AccountService { get; }
        public IConcreteMixService ConcreteMixService {  get; }
        public IAppUserService AppUserService { get; }
        public IClientService ClientService { get; }
        public IAddressService AddressService { get; }
        public IProjectService ProjectService { get; }
        public IMaterialService MaterialService { get; }
        public IOrderService OrderService { get; }
        public ITokenService TokenService { get; }
        public IRefreshTokenService RefreshTokenService { get; }

        public Service(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILoggerFactory loggerFactory, 
            ICacheService cacheService
            )
        {
            logger = loggerFactory.CreateLogger<Service>();

            this.cacheService = cacheService;
            
            AccountService = new AccountService(unitOfWork, mapper, logger);
            ConcreteMixService = new ConcreteMixService(unitOfWork, mapper, logger, cacheService);
            AppUserService = new AppUserService(unitOfWork, mapper, logger, cacheService);
            ClientService = new ClientService(unitOfWork, mapper, logger, cacheService);
            AddressService = new AddressService(unitOfWork, mapper, logger, cacheService);
            ProjectService = new ProjectService(unitOfWork, mapper, logger, cacheService);
            MaterialService = new MaterialService(unitOfWork, mapper, logger, cacheService);
            OrderService = new OrderService(unitOfWork, mapper, logger, cacheService);
            TokenService = new TokenService(unitOfWork, mapper, logger);
            RefreshTokenService = new RefreshTokenService(unitOfWork, mapper, logger);
        }
    }
}
