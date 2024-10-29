using ConcreteIndustry.BLL.Enums;

namespace ConcreteIndustry.BLL.Exceptions
{
    public class ResourceUpdateFailedException : Exception
    {
        public ErrorType Error { get; }
        public string? Resource { get; }
        public long? Id { get; set; }

        public ResourceUpdateFailedException(ErrorType error, string? resource, long? id)
            : base(GetErrorMessage(error, resource, id))
        {
            Error = error;
        }

        private static string GetErrorMessage(ErrorType error, string? resource, long? id) =>
        error switch
        {
            ErrorType.FailedToUpdateResource => $"Failed to update {resource} with id: {id}",
            _ => "Unknow error"
        };
    }
}
