using System;
using System.Collections.Generic;
using API_DbAccess;

namespace api.model {
    public class Order
    {
        public Order()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime BillingAddress { get; set; }
        public DateTime DeliveryAddress { get; set; }
        public bool IsValid { get; set; }
        public int CustomerId { get; set; }

        public virtual API_DbAccess.Customer Customer { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}