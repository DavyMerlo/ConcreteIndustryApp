using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Responses.RefreshTokens
{
    public class RefreshTokenDTO
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
        public DateTime? Revoked { get; set; }
    }
}
