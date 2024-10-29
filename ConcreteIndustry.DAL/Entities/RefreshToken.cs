using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Entities
{
    public class RefreshToken : BaseEntity
    {
        public long UserID { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
        public DateTime? Revoked { get; set; }
    }
}
