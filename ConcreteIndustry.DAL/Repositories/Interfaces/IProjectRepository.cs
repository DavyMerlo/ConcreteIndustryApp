using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project?> GetProjectByIdAsync(long id);
        Task<int> AddProjectAsync(Project project);
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(long id);
    }
}
