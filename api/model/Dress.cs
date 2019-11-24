using System;
using System.Collections.Generic;

namespace api.model {
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
        public bool Availible { get; set; }
        public DateTime DateBeginAvailable { get; set; }
        public DateTime DateEndAvailable { get; set; }
        public int PartnersId { get; set; }

        public virtual Partners Partners { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}