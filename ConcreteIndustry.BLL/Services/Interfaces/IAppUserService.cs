using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using System.Security.Claims;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUserDTO>> GetAll();
        Task<AppUserDTO> GetById(long id);
        Task<AppUserDTO?> Update(UpdateAppUserRequest request);
        Task DeleteById(long id);
    }
}
