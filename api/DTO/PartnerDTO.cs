using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class PartnerDTO
    {
        public PartnerDTO()
        {
            
        }
        [Required]
        public int Id {get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set;}
    }
}