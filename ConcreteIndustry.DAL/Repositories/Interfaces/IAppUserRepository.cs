using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAppUsersAsync();
        Task<AppUser?> GetUserByIdAsync(long id);
        Task<bool> UpdateUserAsync(AppUser appUser);
        Task<bool> DeleteUserAsync(long id);

        Task<int> RegisterUserAsync(AppUser appUser);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<bool> UpdatePasswordAsync(long userId, string password);
        Task<string?> GetPasswordHashByUserId(long userId);
    }
}
