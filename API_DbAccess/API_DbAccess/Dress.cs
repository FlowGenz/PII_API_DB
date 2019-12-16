using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class Dress
    {
        public Dress()
        {
            Favorites = new HashSet<Favorites>();
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public string DressName { get; set; }
        public string Describe { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public DateTime DateBeginAvailable { get; set; }
        public DateTime DateEndAvailable { get; set; }
        public string UrlImage { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
