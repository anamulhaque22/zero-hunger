using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace zero_hunger.EF.Models
{
    public enum UserRole
    {
        Admin,
        Regular
    }
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage ="Provide a valid email address!")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public ICollection<CollectRequest> AssignedCollectRequests { get; set; }
        public ICollection<Distribution> Distributions { get; set; }
    }
}