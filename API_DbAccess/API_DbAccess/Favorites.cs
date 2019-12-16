using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class Favorites
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual User User { get; set; }
    }
}
