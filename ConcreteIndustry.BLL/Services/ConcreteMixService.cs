using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.ConcreteMixes;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class ConcreteMixService : IConcreteMixService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public ConcreteMixService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger,
            ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<ConcreteMixDTO>> GetAll()
        {
            try
            {
                var cachedConcreteMixes = cacheService.GetData<IEnumerable<ConcreteMix>>(CacheSettings.Key.ConcreteMixes);
                if(cachedConcreteMixes != null && cachedConcreteMixes.Any())
                {
                    return mapper.Map<IEnumerable<ConcreteMixDTO>>(cachedConcreteMixes); 
                }

                var concreteMixes = await unitOfWork.ConcreteMixes.GetConcreteMixesAsync();
                if (!concreteMixes.Any()) 
                {
                    logger.LogWarning("{Service} No concretemixes found", typeof(ConcreteMixService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(ConcreteMix), null);
                }

                cacheService.SetData(CacheSettings.Key.ConcreteMixes, concreteMixes, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<ConcreteMixDTO>>(concreteMixes);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get All function error", typeof(ConcreteMixService));
                throw;
            }
        }

        public async Task<ConcreteMixDTO> GetById(long id)
        {
            try
            {
                var cachedConcreteMix = cacheService.GetData<ConcreteMix>(CacheSettings.Key.ConcreteMix);
                if(cachedConcreteMix != null)
                {
                    return mapper.Map<ConcreteMixDTO>(cachedConcreteMix);
                }

                var concreteMix = await unitOfWork.ConcreteMixes.GetConcreteMixByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(ConcreteMix), id);

                cacheService.SetData(CacheSettings.Key.ConcreteMix, concreteMix, CacheSettings.CacheExpirationTime);
                return mapper.Map<ConcreteMixDTO>(concreteMix);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(ConcreteMixService));
                throw;
            }
        }

        public async Task<ConcreteMixDTO?> Add(CreateConcreteMixRequest request)
        {
            try
            {
                var createdConcreteMix = mapper.Map<ConcreteMix>(request);
                var id = await unitOfWork.ConcreteMixes.AddConcreteMixAsync(createdConcreteMix);
                if (id <= 0)
                {
                    throw new ResourceAddFailedException(ErrorType.FailedToCreateResource, nameof(ConcreteMix));
                }
                return await GetById(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Create Error", typeof(ConcreteMixService));
                throw;
            }
        }

        public async Task<ConcreteMixDTO?> Update(UpdateConcreteMixRequest request)
        {
            try
            {
                var updatedConcreteMix = mapper.Map<ConcreteMix>(request);
                bool isUpdated = await unitOfWork.ConcreteMixes.UpdateConcreteMixAsync(updatedConcreteMix);
                if(!isUpdated)
                {
                    logger.LogWarning("{Service} Update Warning", typeof(ConcreteMixService));
                    throw new ResourceUpdateFailedException(ErrorType.FailedToUpdateResource, nameof(ConcreteMix), request.Id);
                }
                return await GetById(updatedConcreteMix.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Update Error", typeof(ConcreteMixService));
                throw;
            }
        }

        public async Task DeleteById(long id)
        {
            try
            {
                bool isDeleted = await unitOfWork.ConcreteMixes.DeleteConcreteMixAsync(id);
                if(!isDeleted)
                {
                    logger.LogWarning("{Service} Delete Warning", typeof(ConcreteMixService));
                    throw new ResourceDeleteFailedException(ErrorType.FailedToDeleteResource, nameof(ConcreteMix), id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Delete Error", typeof(ConcreteMixService));
                throw;
            }
        }
    }
}
