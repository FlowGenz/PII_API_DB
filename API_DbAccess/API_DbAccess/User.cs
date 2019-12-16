using System;
using System.Collections.Generic;

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

        public int Id { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public int? LoyaltyPoints { get; set; }

        public virtual ICollection<Dress> Dress { get; set; }
        public virtual ICollection<DressOrder> DressOrder { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
    }
}
