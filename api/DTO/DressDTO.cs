using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class DressDTO
    {
        public DressDTO()
        {
        }

        [Required] #warning pourquoi required ?
        public int Id { get; set; }
        [Required]
        public string DressName { get; set; }
        [Required]
        public string Describe { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        public DateTime DateBeginAvailable { get; set; }
        public DateTime DateEndAvailable { get; set; }
        [Required]
        public int PartnerId { get; set;}
        [Required]
        public string PartnerName { get; set; }
    }
} 