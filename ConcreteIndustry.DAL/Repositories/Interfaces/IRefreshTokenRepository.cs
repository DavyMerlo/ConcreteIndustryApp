using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenByUserIdAsync(long userId);
        Task<int> AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> IsRefreshTokenValid(string refreshTokenHash);
    }
}
