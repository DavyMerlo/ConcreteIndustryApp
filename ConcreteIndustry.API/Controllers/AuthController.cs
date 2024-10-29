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
            var token = service.TokenService.GenerateUserToken(result.User, out DateTime expDate);
            await service.TokenService.AddUserToken(result.User.Id, token, expDate);
            var response = new ApiResponse<RegisterDTO?>(true, "success", new RegisterDTO
            {
                Token = token,
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

            var accesToken = service.TokenService.GenerateUserToken(result.User, out DateTime expirationDate);
            var refreshToken = service.RefreshTokenService.GenerateRefreshToken();

            await service.TokenService.AddUserToken(result.User.Id, accesToken, expirationDate);
            await service.RefreshTokenService.AddRefreshToken(result.User.Id, refreshToken);

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
            var appUser = service.AccountService.GetUserClaims(User);
            await service.TokenService.HandleToken(appUser.Id);
            var response = new ApiResponse<string>(true, "success", null, 200);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var userClaim = service.AccountService.GetUserClaims(User);

            var isRefreshTokenValid = service.RefreshTokenService.IsRefreshTokenValid(userClaim.Id);

            await service.TokenService.HandleToken(userClaim.Id);

            var accesToken = service.TokenService.GenerateUserToken(userClaim, out DateTime expirationDate);
            var refreshToken = await service.RefreshTokenService.GetRefreshTokenByUserId(userClaim.Id);

            await service.TokenService.AddUserToken(userClaim.Id, accesToken, expirationDate);    

            var response = new ApiResponse<TokenDTO>(true, "succes", 
                new TokenDTO
                {
                    RefreshToken = refreshToken,    
                    Token = accesToken,
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
            var isUpdated = await service.AccountService.ChangePassword(request, User);
            var response = new ApiResponse<string>(true, "success", null, 200);    
            return Ok(response);
        }
    }
}
