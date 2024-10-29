using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Xml;

namespace ConcreteIndustry.API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";

            (int statusCode, string message) = exception switch
            {
                AccessViolationException => ((int)HttpStatusCode.Forbidden, "Access violation error"),
                OutOfMemoryException => ((int)HttpStatusCode.ServiceUnavailable, "System is out of memory"),
                TimeoutException => ((int)HttpStatusCode.RequestTimeout, "Request timed out"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized access"),
                ArgumentNullException => ((int)HttpStatusCode.BadRequest, "Argument cannot be null"),
                ArgumentOutOfRangeException => ((int)HttpStatusCode.BadRequest, "Argument is out of range"),
                InvalidOperationException => ((int)HttpStatusCode.InternalServerError, "Invalid operation error"),
                FileNotFoundException => ((int)HttpStatusCode.NotFound, "File not found"),
                JsonException => ((int)HttpStatusCode.BadRequest, "JSON deserialization error"),
                XmlException => ((int)HttpStatusCode.BadRequest, "XML format error"),
                HttpRequestException => ((int)HttpStatusCode.BadGateway, "Error in HTTP request"),
                SocketException => ((int)HttpStatusCode.ServiceUnavailable, "Socket error"),

                ResourceNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                ResourceAddFailedException => ((int)HttpStatusCode.BadRequest, exception.Message),
                ResourceUpdateFailedException => ((int)HttpStatusCode.BadRequest, exception.Message),
                ResourceDeleteFailedException => ((int)HttpStatusCode.BadRequest, exception.Message),   

                _ => ((int)HttpStatusCode.InternalServerError, "Internal server error")
            };

            var errorDetails = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(errorDetails.ToString(), cancellationToken);

            return true;
        }
    }
}
