using ConcreteIndustry.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ConcreteIndustry.BLL.DTOs.Requests.Validators
{
    public class ClientExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));

            if (unitOfWork == null)
            {
                throw new InvalidOperationException("UnitOfWork is not available.");
            }

            var clientExists = unitOfWork.Clients.DoesClientExist((long)value).Result;

            if (!clientExists)
            {
                return new ValidationResult($"Client ID {(long)value} does not exist.");
            }

            return ValidationResult.Success;
        }
    }
}
