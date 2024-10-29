using System.Text.Json;

namespace ConcreteIndustry.BLL.DTOs.Responses.Api
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
