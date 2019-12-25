using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class DressOrder
    {
        public DressOrder()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        [Required]
        [MaxLength(50)]
        public int Id { get; set; }
        [Required]
        public DateTime BillingDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string BillingAddress { get; set; }
        [Required]
        [MaxLength(50)]
        public string DeliveryAddress { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        [MaxLength(50)]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
