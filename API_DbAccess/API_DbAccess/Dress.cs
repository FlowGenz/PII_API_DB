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
        [MaxLength(50)]
        [MinLength(2)]
        public string DressName { get; set; }
        [Required]
        [MaxLength(200)]
        //ICI EST LA DESCRIPTION, CE COMMENTAIRE DOIT ETRE SUPPRIME
        public string Description { get; set; }
        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateBeginAvailable { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateEndAvailable { get; set; }
        [Required]
        [Url]
        public string UrlImage { get; set; }
        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
