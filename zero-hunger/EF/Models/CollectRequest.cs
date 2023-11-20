using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zero_hunger.EF.Models
{
    public class CollectRequest
    {
        public int Id { get; set; }

        public DateTime RequestedTime { get; set; }
        public string Status { get; set; }
        public DateTime PreservedTime { get; set; }
        public string FoodItem { get; set; }

        // The employee who accepted the request
        public int? AcceptedByEmployeeId { get; set; }
        public Employee AcceptedByEmployee { get; set; }

        // The employee assigned to collect the request
        public int? AssignedEmployeeId { get; set; }
        public Employee AssignedEmployee { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<Distribution> Distributions { get; set; }
    }
}