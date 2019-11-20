using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class Partners
    {
        public Partners()
        {
            Dress = new HashSet<Dress>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PartnerAddress { get; set; }

        public virtual ICollection<Dress> Dress { get; set; }
    }
}
