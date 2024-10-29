using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task<UserToken?> GetUserTokenByUserIdAsync(long userId);
        Task<UserToken?> GetUserTokenByTokenAsync(string token);
        Task<int> AddUserTokenAsync(UserToken token);
        Task<bool> UpdateUserTokenAsync(UserToken token);
    }
}
