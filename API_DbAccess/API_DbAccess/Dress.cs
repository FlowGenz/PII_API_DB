using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DbAccess
{
    public partial class Dress
    {
        public Dress()
        {
            Favorites = new HashSet<Favorites>();
            OrderLine = new HashSet<OrderLine>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string DressName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [RegularExpression("[0-9]{1,4}.[0-9]{2}")]

        public decimal Price { get; set; }
        [Required]
        [MaxLength(5)]
        public string Size { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateBeginAvailable { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateEndAvailable { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Url]
        public string UrlImage { get; set; }
        [Required]
        public string PartnerId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
