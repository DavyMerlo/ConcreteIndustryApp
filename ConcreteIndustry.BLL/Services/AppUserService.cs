using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public AppUserService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger, ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<AppUserDTO>> GetAll()
        {
            try
            {
                var cachedAppUsers = cacheService.GetData<IEnumerable<AppUser>>(CacheSettings.Key.AppUsers);
                if(cachedAppUsers != null && cachedAppUsers.Any())
                {
                    return mapper.Map<IEnumerable<AppUserDTO>>(cachedAppUsers);
                }

                var appUsers = await unitOfWork.AppUsers.GetAppUsersAsync();
                if (!appUsers.Any())
                {
                    logger.LogWarning("{Service} No appusers found", typeof(ClientService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(AppUser), null);
                }
                cacheService.SetData(CacheSettings.Key.AppUsers, appUsers, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Get All function error", typeof(AppUserService));
                throw;
            }
        }

        public async Task<AppUserDTO> GetById(long id)
        {
            try
            {
                var cachedAppUser = cacheService.GetData<AppUser>(CacheSettings.Key.AppUser);
                if (cachedAppUser != null)
                {
                    return mapper.Map<AppUserDTO>(cachedAppUser);
                }

                var appUser = await unitOfWork.AppUsers.GetUserByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(AppUser), id);

                cacheService.SetData(CacheSettings.Key.AppUser, appUser, CacheSettings.CacheExpirationTime);
                return mapper.Map<AppUserDTO>(appUser);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(AppUserService));
                throw;
            }
        }

        public async Task<AppUserDTO?> Update(UpdateAppUserRequest request)
        {
            try
            {
                var updatedAppUser = mapper.Map<AppUser>(request);
                bool isUpdated = await unitOfWork.AppUsers.UpdateUserAsync(updatedAppUser);
                if (!isUpdated)
                {
                    logger.LogWarning("{Service} Update Warning", typeof(AppUserService));
                    throw new ResourceUpdateFailedException(ErrorType.FailedToUpdateResource, nameof(AppUser), request.Id);
                }
                return await GetById(updatedAppUser.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Update Error", typeof(AppUserService));
                throw;
            }
        }

        public async Task DeleteById(long id)
        {
            try
            {
                bool isDeleted = await unitOfWork.AppUsers.DeleteUserAsync(id);
                if (!isDeleted)
                {
                    logger.LogWarning("{Service} Delete Warning", typeof(AppUserService));
                    throw new ResourceDeleteFailedException(ErrorType.FailedToUpdateResource, nameof(AppUser), id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Delete Error", typeof(AppUserService));
                throw;
            }
        }
    }
}
