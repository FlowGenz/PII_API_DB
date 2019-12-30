using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class OrderLineDTO
    {
        public OrderLineDTO()
        {
        }
        public string Id { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateBeginLocation { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateEndLocation { get; set; }
        [Required]
        [Range(0, 9999.99)]
        public decimal FinalPrice { get; set; }
        [Required]
        public string DressName { get; set; }
        [Required]
        public bool IsDressAvailable { get; set; }
        [Required]
        public string DressUrlImage { get; set; }
        [Required]
        public string DressOrderId { get; set; }
        [Required]
        public string DressId { get; set; }
    }
}