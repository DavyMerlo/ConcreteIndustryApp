using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Responses.Materials
{
    public class MaterialDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal PricePerTon { get; set; }
        public long SupplierID { get; set; }
    }
}
