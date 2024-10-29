using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Entities
{
    public class Client : BaseEntity
    {
        public string CompanyName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public long AddressID { get; set; }
    }
}
