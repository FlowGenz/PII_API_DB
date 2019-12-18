using System;

namespace DTO {
    public class FavoriteDTO
    {
        public FavoriteDTO()
        {
        }

        public int Id {get; set;}
        public string UrlImage {get; set;}
        public string DressName {get; set;}
        public Decimal DressPrice {get; set;}
    }
} 