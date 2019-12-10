using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class DressOrder
    {
        public DressOrder()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsValid { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
