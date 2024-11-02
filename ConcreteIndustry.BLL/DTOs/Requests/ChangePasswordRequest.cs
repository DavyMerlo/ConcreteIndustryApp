
using System.ComponentModel.DataAnnotations;

namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot contain more than 255 characters")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot contain more than 255 characters")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot contain more than 255 characters")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
