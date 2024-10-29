
namespace ConcreteIndustry.BLL.DTOs.Responses.Api
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public short StatusCode { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool succes, string message, T? data, short statusCode)
        {
            Success = succes;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
    }
}
