using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<(IEnumerable<Project> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> GetProjectsPaginatedAsync(
            int pageNumber, int pageSize);
        Task<Project?> GetProjectByIdAsync(long id);
        Task<long?> AddProjectAsync(Project project);
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(long id);
    }
}
