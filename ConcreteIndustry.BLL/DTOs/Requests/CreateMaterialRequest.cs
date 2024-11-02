using System.ComponentModel.DataAnnotations;

namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class CreateMaterialRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal PricePerTon { get; set; }

        [Required]
        public long SupplierID { get; set; }
    }
}
