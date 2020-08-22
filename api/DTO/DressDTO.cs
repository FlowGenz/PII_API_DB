using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class DressDTO
    {
        public DressDTO()
        {
        }

        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string DressName { get; set; }
        [Required]
        [MaxLength(50)]
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
        public DateTime DateBeginAvailable { get; set; }
        public DateTime? DateEndAvailable { get; set; }
        [Required]
        [MaxLength(50)]
        public string PartnerId { get; set;}
        [Required]
        [MaxLength(50)]
        public string PartnerName { get; set; }
        [Required]
        [Url]
        [MaxLength(255)]
        public string UrlImage { get; set;}
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
} 