using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDTO>> GetAll();
        Task<(IEnumerable<ProjectDTO> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> GetAllPaginated(int pageNumber, int pageSize);
        Task<ProjectDTO> GetById(long id);
        Task<ProjectDTO?> Add(CreateProjectRequest request);
        Task<ProjectDTO?> Update(UpdateProjectRequest request);
        Task<bool> DeleteById(long id);
    }
}
