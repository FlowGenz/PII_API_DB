using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class Dress
    {
        public Dress()
        {
            Favorites = new HashSet<Favorites>();
            OrderLine = new HashSet<OrderLine>();
        }

        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string DressName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Describe { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,4}.[0-9]{2}")]
        public decimal Price { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        public DateTime DateBeginAvailable { get; set; }
        public DateTime DateEndAvailable { get; set; }
        [Required]
        [Url]
        [MaxLength(255)]
        public string UrlImage { get; set; }
        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
