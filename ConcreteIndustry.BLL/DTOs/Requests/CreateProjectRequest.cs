using ConcreteIndustry.BLL.DTOs.Requests.Validators;
using System.ComponentModel.DataAnnotations;

namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class CreateProjectRequest : IValidatableObject
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters long")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 100 characters long")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [ClientExists]
        public long ClientID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Estimated value must be greater than 0.")]
        public decimal EstimatedValue { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            foreach (var validationResult in ValidateStartAndEndDate())
            {
                yield return validationResult;
            }
        }

        public IEnumerable<ValidationResult> ValidateStartAndEndDate()
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult("End date must be greater than start date", new[] { nameof(EndDate) });
            }
        }
    }
}
