using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetMaterialsAsync();
        Task<Material?> GetMaterialByIdAsync(long id);
        Task<int> AddMaterialAsync(Material material);
        Task<bool> UpdateMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(long id);
    }
}
