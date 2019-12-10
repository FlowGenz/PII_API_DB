using System;

namespace DTO {
    public class DressOrderDTO
    {
        public DressOrderDTO()
        {
            
        }

        public int Id { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsValid { get; set; }
        public int CustomerId { get; set;}
        public string CustomerName { get; set; }
    }
}
