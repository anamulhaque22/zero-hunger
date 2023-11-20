using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zero_hunger.EF.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

        public ICollection<CollectRequest> CollectRequests { get; set; }
    }
}