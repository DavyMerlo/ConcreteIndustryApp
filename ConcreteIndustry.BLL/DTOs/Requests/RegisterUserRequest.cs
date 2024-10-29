using ConcreteIndustry.BLL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 100 characters long")]
        public string? FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 100 characters long")]
        public string? LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 100 characters long")]
        public string? UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100)]
        public string? Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot contain more than 255 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string? Password { get; set; } = string.Empty;

        public UserRole? Role { get; set; }
    }
}
