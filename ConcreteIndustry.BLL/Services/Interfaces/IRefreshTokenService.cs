
namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task<string> GetRefreshTokenByUserId(long userId);
        Task<string> AddRefreshToken(long userId);
        Task HandleRefreshToken(long userId);
        Task<bool> IsRefreshTokenValid(string currentRefreshToken);
    }
}
