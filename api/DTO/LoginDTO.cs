using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class LoginDTO {
        public LoginDTO(){ }


        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        [MaxLength(60)]
        public string Password { get; set; }
    }
}