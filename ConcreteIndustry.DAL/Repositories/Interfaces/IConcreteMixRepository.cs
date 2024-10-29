using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IConcreteMixRepository
    {
        Task<IEnumerable<ConcreteMix>> GetConcreteMixesAsync();
        Task<ConcreteMix?> GetConcreteMixByIdAsync(long id);
        Task<int> AddConcreteMixAsync(ConcreteMix concreteMix);
        Task<bool> UpdateConcreteMixAsync(ConcreteMix concreteMix);
        Task<bool> DeleteConcreteMixAsync(long id);
    }
}
