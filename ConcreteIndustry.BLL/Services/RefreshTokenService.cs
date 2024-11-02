using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.RefreshTokens;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.BLL.Services.Security;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
namespace ConcreteIndustry.BLL.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public RefreshTokenService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public string GenerateRefreshToken()
        {
            return JwtProvider.CreateRefreshToken();
        }

        public async Task<string> GetRefreshTokenByUserId(long userId)
        {
            try
            {
                var userToken = await unitOfWork.RefreshTokens.GetRefreshTokenByUserIdAsync(userId);
                if (userToken == null)
                {
                    logger.LogWarning("No refreshtoken found for user ID: {UserId}", userId);
                    throw new Exception("RefreshToken is invalid");
                }
                return userToken.RefreshTokenHash;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service}  Get User Refresh Token By UserID function error", typeof(RefreshTokenService));
                throw;
            }
        }

        public async Task<string> AddRefreshToken(long userId)
        {
            try
            {
                var refreshTokenDto = new RefreshTokenDTO
                {
                    UserID = userId,
                    RefreshTokenHash = JwtProvider.CreateRefreshToken(),
                    Expired = DateTime.UtcNow.AddDays(7)
                };

                var refreshToken = mapper.Map<RefreshToken>(refreshTokenDto);
                await unitOfWork.RefreshTokens.AddRefreshTokenAsync(refreshToken);
                return refreshTokenDto.RefreshTokenHash;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Add RefreshToken function error", typeof(RefreshTokenService));
                throw;
            }
        }

        public async Task HandleRefreshToken(long userId)
        {
            var refreshToken = await unitOfWork.RefreshTokens.GetRefreshTokenByUserIdAsync(userId);
            var refreshTokenDto = mapper.Map<RefreshTokenDTO>(refreshToken);
            if (refreshTokenDto != null)
            {
                await RevokeRefreshTokenAsync(refreshTokenDto);
            }
        }

        private async Task<bool> RevokeRefreshTokenAsync(RefreshTokenDTO dto)
        {
            try
            {
                if (dto != null)
                {
                    dto.Revoked = DateTime.UtcNow;
                    var refreshToken = mapper.Map<RefreshToken>(dto);
                    await unitOfWork.RefreshTokens.UpdateRefreshTokenAsync(refreshToken);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Revoke Token function error", typeof(RefreshTokenService));
                throw;
            }
        }

        public async Task<bool> IsRefreshTokenValid(string currentRefreshToken)
        {
            try
            {
                return await unitOfWork.RefreshTokens.IsRefreshTokenValid(currentRefreshToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Revoke Token function error", typeof(RefreshTokenService));
                throw;
            }
        }
    }
}
