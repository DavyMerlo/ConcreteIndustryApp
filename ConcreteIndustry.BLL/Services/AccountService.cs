using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Account;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.BLL.Services.Security;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace ConcreteIndustry.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<RegisterDTO> Register(RegisterUserRequest request)
        {
            try
            {
                var user = mapper.Map<AppUser>(request);
                user.HashedPassword = PasswordHasher.HashPassword(request.Password);
                var userId = await unitOfWork.AppUsers.RegisterUserAsync(user);

                var appUser = await GetAppUserById(userId) ?? 
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(AppUser), userId);

                var userRespone = mapper.Map<AppUserDTO>(appUser);
                return new RegisterDTO
                {
                    User = userRespone,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Register function error", typeof(AppUserService));
                throw;
            }
        }

        public async Task<AuthenticationDTO> Login(LoginUserRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var user = await unitOfWork.AppUsers.GetUserByUsernameAsync(request.Email);
                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.HashedPassword))
                {
                    throw new UnauthorizedAccessException();
                }
                var userResponse = mapper.Map<AppUserDTO>(user);
                return new AuthenticationDTO
                {
                    User = userResponse,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Login function error", typeof(AppUserService));
                throw;
            }
            finally
            {
                stopwatch.Stop();
                var elapsed = stopwatch.Elapsed;
                Console.WriteLine($"Login took: {elapsed.TotalMilliseconds} ms");
            }
        }

        public AppUserDTO GetUserClaims(ClaimsPrincipal user)
        {
            try
            {
                var identity = GetUserIdentity(user) ?? 
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(ClaimsIdentity), null);

                var userIdClaim = identity.FindFirst("userid")?.Value;
                var userNameClaim = identity.FindFirst($"username")?.Value;
                var firstNameClaim = identity.FindFirst("firstname")?.Value;
                var lastNameClaim = identity.FindFirst("lastname")?.Value;
                var emailClaim = identity.FindFirst("emailaddress")?.Value;
                var userRoleClaim = identity.FindFirst(ClaimTypes.Role)?.Value;
                var expired = identity.FindFirst("expire")?.Value;

                return new AppUserDTO
                {
                    Id = long.Parse(userIdClaim),
                    FirstName = firstNameClaim,
                    LastName = lastNameClaim,
                    UserName = userNameClaim,
                    Email = emailClaim,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), userRoleClaim)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get User Claims Error", typeof(AppUserService));
                throw;
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal user)
        {
            try
            {
                var identity = GetUserIdentity(user) ?? 
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(ClaimsIdentity), null);

                var userIdClaim = long.Parse(identity.FindFirst("userid")?.Value ?? 
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, identity.FindFirst("userid")?.Value, null));

                var appUser = await GetAppUserById(userIdClaim) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(AppUser), null);

                VerifyCurrentPassword(request.CurrentPassword, appUser.HashedPassword);
                ValidatePasswords(request.NewPassword, request.ConfirmNewPassword);

                bool isUpdated = await unitOfWork.AppUsers.UpdatePasswordAsync(userIdClaim, PasswordHasher.HashPassword(request.NewPassword));
                if (!isUpdated)
                {
                    logger.LogWarning("{Service} Update Password Warning", typeof(AppUserService));
                    throw new ResourceUpdateFailedException(ErrorType.FailedToUpdateResource, nameof(AppUser), appUser.Id);
                }
                return true;
            }
            catch (Exception ex )
            {
                logger.LogError(ex,"{Service} Change Password function error", typeof(AppUserService));
                throw;
            }
        }

        public async Task<bool> IsPassWordValid(ClaimsPrincipal user, string password)
        {
            try
            {
                var identity = GetUserIdentity(user) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(ClaimsIdentity), null);

                var userId = long.Parse(identity.FindFirst("userid")?.Value ??
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, identity.FindFirst("userid")?.Value, null));

                string? currentHashedPassword = await unitOfWork.AppUsers.GetPasswordHashByUserId(userId) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, $"No password found with Id: {userId}", null);

                VerifyCurrentPassword(password, currentHashedPassword);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Check if Password is valid error", typeof(AppUserService));
                throw;
            }
        }

        private ClaimsIdentity GetUserIdentity(ClaimsPrincipal user)
        {
            if (user.Identity is not ClaimsIdentity claims)
            {
                throw new InvalidOperationException($"{nameof(ClaimsPrincipal)} is either null or not a {nameof(ClaimsIdentity)}");
            }
            return claims;
        }

        private async Task<AppUser> GetAppUserById(long userId)
        {
            var appUser = await unitOfWork.AppUsers.GetUserByIdAsync(userId) ??
                throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(AppUser), userId);
            return appUser;
        }

        private void ValidatePasswords(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                throw new InvalidOperationException("New Password and the confirm Password do not match.");
            }
        }

        private void VerifyCurrentPassword(string currentPassword, string hashedPassword)
        {
            if (!PasswordHasher.VerifyPassword(currentPassword, hashedPassword))
            {
                throw new InvalidOperationException("Current password is incorrect.");
            }
        }
    }
}
