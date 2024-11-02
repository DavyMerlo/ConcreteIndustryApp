using ConcreteIndustry.BLL.DTOs.Responses.Users;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateUserToken(AppUserDTO? appUser, out DateTime expirationDat);
        Task<string> GetUserTokenByUserId(long userId);
        Task<string> AddUserToken(AppUserDTO? userDTO);
        Task HandleToken(long userId);
    }
}
