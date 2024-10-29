using ConcreteIndustry.BLL.DTOs.Responses.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAll();
        Task<AddressDTO> GetById(long id);
    }
}
