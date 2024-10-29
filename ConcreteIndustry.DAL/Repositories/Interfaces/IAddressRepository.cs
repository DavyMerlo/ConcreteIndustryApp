using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAddressesAsync();
        Task<Address?> GetAddressByIdAsync(long id);
        Task<int> AddAddressAsync(Address address);
        Task<bool> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(long id);
    }
}
