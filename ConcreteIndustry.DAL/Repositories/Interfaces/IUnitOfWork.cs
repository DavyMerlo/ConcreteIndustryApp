using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IConcreteMixRepository ConcreteMixes { get; }
        IAppUserRepository AppUsers { get; }
        IClientRepository Clients { get; }
        IAddressRepository Addresses { get; }
        IProjectRepository Projects { get; }
        IOrderRepository Orders { get; }
        IMaterialRepository Materials { get; }
        ISupplierRepository Suppliers { get; }
        ITokenRepository Tokens { get; }
        IRefreshTokenRepository RefreshTokens { get; }
    }
}
