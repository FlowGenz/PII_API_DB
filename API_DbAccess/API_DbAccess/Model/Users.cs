using System;
using System.Collections.Generic;

namespace API_DbAccess
{
    public partial class Users
    {
        public Users()
        {
            Customer = new HashSet<Customer>();
            Partners = new HashSet<Partners>();
        }

        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string Privilege { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Partners> Partners { get; set; }
    }
}
