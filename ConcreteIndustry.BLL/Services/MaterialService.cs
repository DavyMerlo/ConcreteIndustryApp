using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Materials;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public MaterialService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger logger,
            ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<MaterialDTO>> GetAll()
        {
            try
            {
                var cachedMaterials = cacheService.GetData<IEnumerable<Material>>(CacheSettings.Key.Materials);
                if (cachedMaterials != null)
                {
                    return mapper.Map<IEnumerable<MaterialDTO>>(cachedMaterials);
                }

                var materials = await unitOfWork.Materials.GetMaterialsAsync();
                if (!materials.Any())
                {
                    logger.LogWarning("{Service} No materials found", typeof(MaterialService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Material), null);
                }

                cacheService.SetData(CacheSettings.Key.Materials, materials, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<MaterialDTO>>(materials);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get All function error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<MaterialDTO> GetById(long id)
        {
            try
            {
                var cachedMaterial = cacheService.GetData<Material>(CacheSettings.Key.Material);
                if (cachedMaterial != null)
                {
                    return mapper.Map<MaterialDTO>(cachedMaterial);
                }
                var material = await unitOfWork.Materials.GetMaterialByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(Material), id);

                cacheService.SetData(CacheSettings.Key.Material, material, CacheSettings.CacheExpirationTime);
                return mapper.Map<MaterialDTO>(material);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(MaterialService));
                throw;
            }
        }

        public async Task<MaterialDTO?> Add(CreateMaterialRequest request)
        {
            try
            {
                var createdMaterial = mapper.Map<Material>(request);
                var id = await unitOfWork.Materials.AddMaterialAsync(createdMaterial);
                if (id <= 0)
                {
                    throw new ResourceAddFailedException(ErrorType.FailedToCreateResource, nameof(Material));
                }
                return await GetById(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} Create Error", typeof(MaterialService));
                throw;
            }
        }
    }
}
