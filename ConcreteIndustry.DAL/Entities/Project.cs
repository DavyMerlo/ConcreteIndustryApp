using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public long ClientID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
