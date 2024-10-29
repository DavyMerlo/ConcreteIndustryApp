

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IService
    {
        IAccountService AccountService { get; }
        IConcreteMixService ConcreteMixService { get; }
        IAppUserService AppUserService { get; }
        IClientService ClientService { get; }
        IAddressService AddressService { get; }
        IProjectService ProjectService { get; }
        IMaterialService MaterialService { get; }
        IOrderService OrderService { get; }
        ITokenService TokenService { get; }
        IRefreshTokenService RefreshTokenService { get; }
    }
}
