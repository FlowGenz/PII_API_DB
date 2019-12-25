using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class User
    {
        public User()
        {
            Dress = new HashSet<Dress>();
            DressOrder = new HashSet<DressOrder>();
            Favorites = new HashSet<Favorites>();
        }

        [Required]
        [MaxLength(50)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(60)]
        [PasswordPropertyText]
        public string UserPassword { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserAddress { get; set; }
        [RegularExpression("[0-9]{1,4}")]
        public int? LoyaltyPoints { get; set; }

        public virtual ICollection<Dress> Dress { get; set; }
        public virtual ICollection<DressOrder> DressOrder { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
    }
}
