using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DbAccess
{
    public partial class Favorites
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DressId { get; set; }

        public virtual Dress Dress { get; set; }
        public virtual User User { get; set; }

    }
}
