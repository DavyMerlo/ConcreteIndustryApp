using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
