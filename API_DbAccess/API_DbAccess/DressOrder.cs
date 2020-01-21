using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DbAccess
{
    public partial class DressOrder
    {
        public DressOrder()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BillingDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string BillingAddress { get; set; }
        [Required]
        [MaxLength(100)]
        public string DeliveryAddress { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        public string UserId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
