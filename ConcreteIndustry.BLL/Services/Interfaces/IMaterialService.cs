using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Clients;
using ConcreteIndustry.BLL.DTOs.Responses.Materials;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialDTO>> GetAll();
        Task<MaterialDTO> GetById(long id);
        Task<MaterialDTO?> Add(CreateMaterialRequest request);
    }
}
