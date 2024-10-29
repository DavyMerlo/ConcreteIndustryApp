using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Account;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using System.Security.Claims;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterDTO> Register(RegisterUserRequest request);
        Task<AuthenticationDTO> Login(LoginUserRequest request);
        AppUserDTO GetUserClaims(ClaimsPrincipal user);
        Task<bool> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal user);
    }
}
