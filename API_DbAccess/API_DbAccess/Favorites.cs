using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class Favorites
    {
        [Required]
        [MaxLength(50)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public int DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual User User { get; set; }
    }
}
