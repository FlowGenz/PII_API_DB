using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class DressOrderDTO
    {
        public DressOrderDTO()
        {
            OrderLines = new List<OrderLineDTO>();
        }
        public string Id { get; set; }
        public DateTime? BillingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public string BillingAddress { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        public string CustomerId { get; set;}
        [Required]
        [MaxLength(50)]
        public string CustomerName { get; set; }
        public IList<OrderLineDTO> OrderLines { get; set; }
    }
}
