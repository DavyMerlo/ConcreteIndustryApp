using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Requests
{
    public class LoginUserRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
        [MaxLength(255, ErrorMessage = "Password cannot contain more than 255 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
