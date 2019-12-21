using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {
    public class CustomerDTO
    {
        public CustomerDTO()
        {

        }

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        [MaxLength(60)]
        public string CustomerPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string CustomerAddress { get; set; }
        [DefaultValue(0)]
        public int LoyaltyPoints { get; set; }
    }
}