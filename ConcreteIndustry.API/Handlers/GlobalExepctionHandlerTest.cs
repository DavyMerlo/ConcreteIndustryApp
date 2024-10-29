using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ConcreteIndustry.API.Handlers
{
    public class GlobalExepctionHandlerTest : IExceptionHandler
    {
        private readonly ILogger<GlobalExepctionHandlerTest> logger;

        public GlobalExepctionHandlerTest(ILogger<GlobalExepctionHandlerTest> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            var details = new ProblemDetails()
            {
                Detail = $"API Error {exception.Message}",
                Instance = "API",
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "API Error",
                Type = "Server Error"
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;

        }
    }
}
