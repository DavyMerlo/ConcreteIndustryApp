using ConcreteIndustry.BLL.DTOs.Responses.Users;
using System.Security.Claims;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateUserToken(AppUserDTO? appUser, out DateTime expirationDat);
        Task<string> GetUserTokenByUserId(long userId);
        Task AddUserToken(long userId, string token, DateTime expired);
        Task HandleToken(long userId);
    }
}
