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
        public int Id {get; set;}
        [Required]
        [Url]
        [MaxLength(255)]
        public string UrlImage {get; set;}
        [Required]
        [MaxLength(50)]
        public string DressName {get; set;}
        public decimal DressPrice {get; set;}
    }
} 