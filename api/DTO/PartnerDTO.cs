using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class PartnerDTO
    {
        public PartnerDTO()
        {
            
        }
        public int Id {get; set;}
        [Required]
        public string Username { get; set;}
    }
}