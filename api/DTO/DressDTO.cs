using System;

namespace DTO {
    public class Dress
    {
        public Dress()
        {
        }

        public int Id { get; set; }
        public string DressName { get; set; }
        public string Describe { get; set; }
        public decimal Price { get; set; }
        public bool Availible { get; set; }
        public DateTime DateBeginAvailable { get; set; }
        public DateTime DateEndAvailable { get; set; }
        public int PartnerId { get; set;}
        public string PartnerName { get; set; }
    }
} 