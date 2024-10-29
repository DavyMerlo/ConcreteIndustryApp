using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataConnection dbConnection;
        private readonly ILogger<UnitOfWork> logger;

        public IConcreteMixRepository ConcreteMixes { get; private set; }
        public IAppUserRepository AppUsers { get; private set; }
        public IClientRepository Clients { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IMaterialRepository Materials { get; private set; }
        public ISupplierRepository Suppliers { get; private set; }
        public ITokenRepository Tokens { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }

        public UnitOfWork(IDataConnection dbConnection, ILoggerFactory loggerFactory)
        {
            this.dbConnection = dbConnection;
            this.logger = loggerFactory.CreateLogger<UnitOfWork>();

            ConcreteMixes = new ConcreteMixRepository(this.dbConnection, this.logger);
            AppUsers = new AppUserRepository(this.dbConnection, this.logger);
            Clients = new ClientRepository(this.dbConnection, this.logger);
            Addresses = new AddressRepository(this.dbConnection, this.logger);
            Projects = new ProjectRepository(this.dbConnection, this.logger);
            Orders = new OrderRepository(this.dbConnection, this.logger);
            Materials = new MaterialRepository(this.dbConnection, this.logger);
            Suppliers = new SupplierRepository(this.dbConnection, this.logger);
            Tokens = new TokenRepository(this.dbConnection, this.logger);
            RefreshTokens = new RefreshTokenRepository(this.dbConnection, this.logger);
        }
    }
}
