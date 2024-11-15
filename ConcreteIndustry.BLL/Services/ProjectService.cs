using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;
using ConcreteIndustry.BLL.Enums;
using ConcreteIndustry.BLL.Exceptions;
using ConcreteIndustry.BLL.Services.Caching;
using ConcreteIndustry.BLL.Services.Interfaces;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcreteIndustry.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;

        public ProjectService(
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

        public async Task<IEnumerable<ProjectDTO>> GetAll()
        {
            try
            {
                var cachedProjects = cacheService.GetData<IEnumerable<Project>>(CacheSettings.Key.Projects);
                if(cachedProjects != null)
                {
                    return mapper.Map<IEnumerable<ProjectDTO>>(cachedProjects);
                }

                var projects = await unitOfWork.Projects.GetProjectsAsync();
                if (!projects.Any())
                {
                    logger.LogWarning("{Service} No projects found", typeof(ProjectService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Project), null);
                }

                cacheService.SetData(CacheSettings.Key.Projects, projects, CacheSettings.CacheExpirationTime);
                return mapper.Map<IEnumerable<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} All function error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<(IEnumerable<ProjectDTO> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var(projects, totalCount, totalPages, hasNext, hasPrevious) = await unitOfWork.Projects.GetProjectsPaginatedAsync(pageNumber, pageSize);
                if (!projects.Any())
                {
                    logger.LogWarning("{Service} No projects found", typeof(ProjectService));
                    throw new ResourceNotFoundException(ErrorType.ResourceNotFound, nameof(Project), null);
                }
                var projectDto = mapper.Map<IEnumerable<ProjectDTO>>(projects);
                return (projectDto, totalCount, totalPages, hasNext, hasPrevious);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Service} All function error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<ProjectDTO> GetById(long id)
        {
            try
            {
                var cachedProject = cacheService.GetData<Project>($"{CacheSettings.Key.Project}:{id}");
                if (cachedProject != null && cachedProject.DeletedAt == null)
                {
                    return mapper.Map<ProjectDTO>(cachedProject);
                }

                var project = await unitOfWork.Projects.GetProjectByIdAsync(id) ??
                    throw new ResourceNotFoundException(ErrorType.ResourceWithIdNotFound, nameof(Project), id);

                cacheService.SetData(CacheSettings.Key.Project, project, CacheSettings.CacheExpirationTime);
                return mapper.Map<ProjectDTO>(project);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Get By Id function error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<ProjectDTO?> Add(CreateProjectRequest request)
        {
            try
            {
                var createdProject = mapper.Map<Project>(request);
                var id = await unitOfWork.Projects.AddProjectAsync(createdProject);
                if(id == null)
                {
                    throw new ResourceAddFailedException(ErrorType.FailedToCreateResource, nameof(Project));
                }

                cacheService.RemoveData(CacheSettings.Key.Projects);
                return await GetById((long)id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Create Error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<ProjectDTO?> Update(UpdateProjectRequest request)
        {
            try
            {
                var updatedProject = mapper.Map<Project>(request);
                var isUpdated = await unitOfWork.Projects.UpdateProjectAsync(updatedProject);
                if (!isUpdated)
                {
                    logger.LogWarning("{Service} Update Warning", typeof(ProjectService));
                    throw new ResourceUpdateFailedException(ErrorType.FailedToUpdateResource, nameof(Project), request.Id);
                }
                cacheService.RemoveData(CacheSettings.Key.Project);
                cacheService.RemoveData(CacheSettings.Key.Projects);
                return await GetById(request.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Update Error", typeof(ProjectService));
                throw;
            }
        }

        public async Task<bool> DeleteById(long id)
        {
            try
            {
                cacheService.RemoveData(CacheSettings.Key.Project);
                cacheService.RemoveData(CacheSettings.Key.Projects);
                bool isDeleted = await unitOfWork.Projects.DeleteProjectAsync(id);
                if (!isDeleted)
                {
                    logger.LogWarning("{Service} Delete Warning", typeof(ProjectService));
                    throw new ResourceDeleteFailedException(ErrorType.FailedToDeleteResource, nameof(Project), id);
                }
                
                return isDeleted;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"{Service} Delete Error", typeof(ProjectService));
                throw;
            }
        }
    }
}
