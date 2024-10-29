
namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class UpdateProjectRequest
    {
        public UpdateProjectRequest()
        {
            
        }

        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public long ClientID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
