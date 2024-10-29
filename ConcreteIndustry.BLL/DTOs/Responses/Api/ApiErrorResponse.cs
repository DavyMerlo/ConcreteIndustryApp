using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.BLL.DTOs.Responses.Api
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? StatusPhrase { get; set; }
        public List<ValidationError> Errors { get; set; } = new();
        public DateTime TimeStamp { get; set; }

        public static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var apiError = new ApiErrorResponse();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.TimeStamp = DateTime.UtcNow;
            var errors = context.ModelState.AsEnumerable();

            foreach (var error in errors)
            {
                foreach (var inner in error.Value!.Errors)
                {
                    apiError.Errors.Add(new ValidationError(error.Key, inner.ErrorMessage));
                }
            }
            return new BadRequestObjectResult(apiError);
        }
    }
}
