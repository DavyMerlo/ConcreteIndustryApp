using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Responses.ConcreteMixes
{
    public class ConcreteMixDTO
    {
        public string Name { get; set; } = string.Empty;
        public string StrengthClass { get; set; } = string.Empty;
        public decimal? MaxAggregateSize { get; set; }
        public decimal? WaterCementRatio { get; set; }
        public string Application { get; set; } = string.Empty;
        public decimal PricePerM3 { get; set; }
    }
}
