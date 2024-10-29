using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcreteIndustry.BLL.DTOs.Responses.Users;

namespace ConcreteIndustry.BLL.DTOs.Responses.Account
{
    public class AuthenticationDTO
    {
        public string AccesToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public AppUserDTO User { get; set; } = new AppUserDTO();
    }
}
