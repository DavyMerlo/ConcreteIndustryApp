using ConcreteIndustry.BLL.Enums;

namespace ConcreteIndustry.BLL.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ErrorType Error { get; }
        public string? Resource { get; }
        public long? Id { get; set; }

        public ResourceNotFoundException(ErrorType error, string? resource, long? id)
            : base(GetErrorMessage(error, resource, id))
        {
            Error = error;
            Resource = resource;
            Id = id;
        }

        private static string GetErrorMessage(ErrorType error, string? resource, long? id) =>
        error switch
        {
            ErrorType.ResourceWithIdNotFound => $"{resource} with id: {id} not found",
            ErrorType.ResourceNotFound => $"{resource} not found",
            _ => "Unknown error"
        };
    }
}
