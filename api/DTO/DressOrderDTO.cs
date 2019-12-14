using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class DressOrderDTO
    {
        public DressOrderDTO()
        {
            
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime BillingDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public DateTime BillingAddress { get; set; }
        [Required]
        public DateTime DeliveryAddress { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        public int CustomerId { get; set;}
        public string CustomerName { get; set; }
    }
}
