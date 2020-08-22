using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {

        }

        public string Id { get; set; }

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
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,20}$")]
        public string CustomerPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string CustomerAddress { get; set; }
        [Required]
        [DefaultValue(0)]
        [RegularExpression("[0-9]{1,4}")]
        public int LoyaltyPoints { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}