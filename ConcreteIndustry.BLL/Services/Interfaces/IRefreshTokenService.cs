
using System.Security.Claims;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        string GetRefreshTokenByClaim(ClaimsPrincipal claim);
        Task<string> GetRefreshTokenByUserId(long userId);
        Task AddRefreshToken(long userId, string tokenHash);
        Task HandleRefreshToken(long userId);
        Task IsRefreshTokenValid(long userId);
    }
}
