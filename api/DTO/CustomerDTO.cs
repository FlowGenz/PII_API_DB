using System;

namespace DTO {
    public class CustomerDTO
    {
        public CustomerDTO()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string CustomerPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public int FidelityPoints { get; set; }
    }
}