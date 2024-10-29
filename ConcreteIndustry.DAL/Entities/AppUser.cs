using ConcreteIndustry.DAL.Enums;

namespace ConcreteIndustry.DAL.Entities
{
    public class AppUser : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public Roles Role { get; set; }
        public DateTime? LastLoginAt { get; set; } 
    }
}
