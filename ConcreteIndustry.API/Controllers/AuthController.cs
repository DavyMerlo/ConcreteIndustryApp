using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Account;
using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.Auth;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IService service) : base(service)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await service.AccountService.Register(request);
            var accesToken = await service.TokenService.AddUserToken(result.User);
            var response = new ApiResponse<RegisterDTO?>(true, "success", new RegisterDTO
            {
                AccesToken = accesToken,
                User = result.User
            },  200);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await service.AccountService.Login(request);
            await service.TokenService.HandleToken(result.User.Id);
            await service.RefreshTokenService.HandleRefreshToken(result.User.Id);
            var accesToken = await service.TokenService.AddUserToken(result.User);
            var refreshToken = await service.RefreshTokenService.AddRefreshToken(result.User.Id);

            var response = new ApiResponse<AuthenticationDTO?>(true, "success", 
                new AuthenticationDTO
                {
                    AccesToken = accesToken,
                    RefreshToken = refreshToken,
                    User = result.User
                },  200);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = service.AccountService.GetUserClaims(User);
            await service.TokenService.HandleToken(user.Id);
            await service.RefreshTokenService.HandleRefreshToken(user.Id);
            var response = new ApiResponse<string>(true, "success", null, 200);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = service.AccountService.GetUserClaims(User);
            var currentRefreshToken = await service.RefreshTokenService.GetRefreshTokenByUserId(user.Id);
            var isCurrentRefreshTokenValid = await service.RefreshTokenService.IsRefreshTokenValid(currentRefreshToken);
            await service.TokenService.HandleToken(user.Id);
            var accesToken = await service.TokenService.AddUserToken(user);

            var response = new ApiResponse<TokenDTO>(true, "succes", 
                new TokenDTO
                {
                    AccesToken = accesToken,
                    RefreshToken = currentRefreshToken,    
                },  200);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("claims")]
        public IActionResult GetUserClaims()
        {
            var claims = service.AccountService.GetUserClaims(User);
            var response = new ApiResponse<AppUserDTO>(true, "success", claims, 200);
            return Ok(response);
        }

        [Authorize] 
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var test = await service.AccountService.IsPassWordValid(User, request.CurrentPassword);
            var isUpdated = await service.AccountService.ChangePassword(request, User);
            var response = new ApiResponse<string>(true, "success", null, 200);    
            return Ok(response);
        }
    }
}
