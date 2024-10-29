using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Responses.Orders
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public long ProjectID { get; set; }
        public long ClientID { get; set; }
        public long ConcreteMixID { get; set; }
        public decimal Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
