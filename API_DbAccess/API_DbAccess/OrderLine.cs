using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class OrderLine
    {
        public string Id { get; set; }
        [Required] 
        public DateTime DateBeginLocation { get; set; }
        [Required] 
        public DateTime DateEndLocation { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,4}.[0-9]{2}")]
        public decimal FinalPrice { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DressOrderId { get; set; }
        [Required]
        public string DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual DressOrder DressOrder { get; set; }
    }
}
