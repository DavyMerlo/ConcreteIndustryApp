using ConcreteIndustry.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(long id);
        Task<int> AddSupplierAsync(Supplier supplier);
        Task<bool> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(long id);
    }
}
