using ConcreteIndustry.BLL.Enums;

namespace ConcreteIndustry.BLL.DTOs.Responses.Users
{
    public class AppUserDTO
    {
        public long Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
