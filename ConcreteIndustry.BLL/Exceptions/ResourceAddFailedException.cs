using ConcreteIndustry.BLL.Enums;

namespace ConcreteIndustry.BLL.Exceptions
{
    public class ResourceAddFailedException : Exception
    {
        public ErrorType Error { get; }
        public string? Resource { get; }

        public ResourceAddFailedException(ErrorType error, string? resource)
            : base(GetErrorMessage(error, resource))
        {
            Error = error;
        }

        private static string GetErrorMessage(ErrorType error, string? resource) =>
         error switch
         {
             ErrorType.FailedToCreateResource => $"Failed to create {resource}",
             _ => "Unknow error"
         };
    }
}
