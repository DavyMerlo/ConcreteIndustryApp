using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Entities
{
    public class Material : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal PricePerTon { get; set; }
        public long SupplierID { get; set; }
    }
}
