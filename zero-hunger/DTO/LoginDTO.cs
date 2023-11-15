using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zero_hunger.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Provide a valid email address!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage ="Password is required!")]
        public string Password { get; set; }
    }
}