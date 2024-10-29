using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Entities
{
    public class Order : BaseEntity
    {
        public long ProjectID { get; set; }
        public long ClientID { get; set; }
        public long ConcreteMixID { get; set; }
        public decimal Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
