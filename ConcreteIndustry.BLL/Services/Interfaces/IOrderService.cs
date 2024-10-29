using ConcreteIndustry.BLL.DTOs.Responses.Orders;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAll();
        Task<OrderDTO> GetById(long id);
    }
}
