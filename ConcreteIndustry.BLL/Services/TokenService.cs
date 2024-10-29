using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.DTOs.Responses.UserTokens;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.BLL.Services.Security;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public TokenService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public string GenerateUserToken(AppUserDTO? appUser, out DateTime expirationDate)
        {
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser), "AppUser cannot be null.");
            }
            var user = mapper.Map<AppUser>(appUser);
            return JwtProvider.CreateToken(user, out expirationDate);
        }

        public async Task<string> GetUserTokenByUserId(long userId)
        {
            try
            {
                var userToken = await unitOfWork.Tokens.GetUserTokenByUserIdAsync(userId);
                if (userToken == null)
                {
                    logger.LogWarning("No token found for user ID: {UserId}", userId);
                    throw new Exception("Token is invalid");
                }
                return userToken.Token;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Get User Token By UserID function error", typeof(TokenService));
                throw;
            }
        }

        public async Task AddUserToken(long userId, string token, DateTime expired)
        {
            try
            {
                var userTokenDto = new UserTokenDTO
                {
                    UserID = userId,
                    Token = token,
                    Expired = expired
                };
                var userToken = mapper.Map<UserToken>(userTokenDto);
                await unitOfWork.Tokens.AddUserTokenAsync(userToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Add UserToken function error", typeof(TokenService));
                throw;
            }
        }

        public async Task HandleToken(long userId)
        {
            var userToken = await unitOfWork.Tokens.GetUserTokenByUserIdAsync(userId);
            var userTokenDto = mapper.Map<UserTokenDTO>(userToken);
            if (userTokenDto != null)
            {
                await RevokeTokenAsync(userTokenDto);
            }
        }

        private async Task<bool> RevokeTokenAsync(UserTokenDTO userTokenDto)
        {
            try
            {
                if (userTokenDto != null)
                {
                    userTokenDto.Revoked = DateTime.UtcNow;
                    var userToken = mapper.Map<UserToken>(userTokenDto);
                    await unitOfWork.Tokens.UpdateUserTokenAsync(userToken);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Revoke Token function error", typeof(TokenService));
                throw;
            }
        }
    }
}
