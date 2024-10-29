using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.ConcreteMixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IConcreteMixService
    {
        Task<IEnumerable<ConcreteMixDTO>> GetAll();
        Task<ConcreteMixDTO> GetById(long id);
        Task<ConcreteMixDTO?> Add(CreateConcreteMixRequest request);
        Task<ConcreteMixDTO?> Update(UpdateConcreteMixRequest request);
        Task DeleteById(long id);
    }
}
