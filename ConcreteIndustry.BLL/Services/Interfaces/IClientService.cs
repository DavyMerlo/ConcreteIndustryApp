using ConcreteIndustry.BLL.DTOs.Responses.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDTO>> GetAll();
        Task<ClientDTO> GetById(long id);
    }
}
