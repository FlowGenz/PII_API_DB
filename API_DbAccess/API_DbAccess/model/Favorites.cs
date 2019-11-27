using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class Favorites
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DressId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Dress Dress { get; set; }
    }
}
