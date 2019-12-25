using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class DressOrderDTO
    {
        public DressOrderDTO()
        {
            
        }
        public int Id { get; set; }
        [Required]
        public DateTime BillingDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public string BillingAddress { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        public int CustomerId { get; set;}
        [Required]
        [MaxLength(50)]
        public string CustomerName { get; set; }
    }
}
