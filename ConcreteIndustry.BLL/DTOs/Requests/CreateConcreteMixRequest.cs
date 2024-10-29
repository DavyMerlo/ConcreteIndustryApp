
namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class CreateConcreteMixRequest
    {
        public string Name { get; set; } = string.Empty;
        public string StrengthClass { get; set; } = string.Empty;
        public decimal? MaxAggregateSize { get; set; }
        public decimal? WaterCementRatio { get; set; }
        public string Application { get; set; } = string.Empty;
        public decimal PricePerM3 { get; set; }
    }
}
