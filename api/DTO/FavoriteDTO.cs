using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class FavoriteDTO
    {
        public FavoriteDTO()
        {
        }

        [Required]
        public string Id {get; set;}
        [Required]
        [Url]
        [MaxLength(255)]
        public string UrlImage {get; set;}
        [Required]
        [MaxLength(50)]
        public string DressName {get; set;}
        [Required]
        [RegularExpression("[0-9]{1,4}.[0-9]{2}")]
        public decimal DressPrice {get; set;}
        [Required]
        public string DressId { get; set;}
        [Required]
        public string CustomerId {get; set;}
    }
} 