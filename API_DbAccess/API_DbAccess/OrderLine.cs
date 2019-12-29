using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DbAccess
{
    public partial class OrderLine
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public string Id { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateBeginLocation { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateEndLocation { get; set; }
        [Required]
        [Range(0, 9999.99)]
        public decimal FinalPrice { get; set; }
        [Required]
        public string DressOrderId { get; set; }
        [Required]
        public string DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual DressOrder DressOrder { get; set; }
    }
}
