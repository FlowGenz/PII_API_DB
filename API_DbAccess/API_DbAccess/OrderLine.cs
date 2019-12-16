using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class OrderLine
    {
        public int Id { get; set; }
        public DateTime DateBeginLocation { get; set; }
        public DateTime DateEndLocation { get; set; }
        public decimal FinalPrice { get; set; }
        public int UserId { get; set; }
        public int DressOrderId { get; set; }
        public int DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual DressOrder DressOrder { get; set; }
    }
}
