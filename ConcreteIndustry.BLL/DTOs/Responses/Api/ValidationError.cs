
namespace ConcreteIndustry.BLL.DTOs.Responses.Api
{
    public class ValidationError
    {
        public string field { get; set; }
        public string message { get; set; }

        public ValidationError(string field, string message)
        {
            this.field = field;
            this.message = message;
        }
    }
}
