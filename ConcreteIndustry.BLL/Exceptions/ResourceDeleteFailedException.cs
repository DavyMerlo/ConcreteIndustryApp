using ConcreteIndustry.BLL.Enums;

namespace ConcreteIndustry.BLL.Exceptions
{
    public class ResourceDeleteFailedException : Exception
    {
        public ErrorType Error { get; }
        public string? Resource { get; }
        public long? Id { get; set; }

        public ResourceDeleteFailedException(ErrorType error, string? resource, long? id)
            : base(GetErrorMessage(error, resource, id))
        {
            Error = error;
        }

        private static string GetErrorMessage(ErrorType error, string? resource, long? id) =>
        error switch
        {
            ErrorType.FailedToDeleteResource => $"Failed to delete {resource} with id: {id}",
            _ => "Unknow error"
        };
    }
}
