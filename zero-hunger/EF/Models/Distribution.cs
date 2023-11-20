using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zero_hunger.EF.Models
{
    public class Distribution
    {
        public int Id { get; set; }

        public int CollectRequestId { get; set; }
        public CollectRequest CollectRequest { get; set; }

        public string Area { get; set; }

        public int DistributedById { get; set; }  
        public Employee DistributedBy { get; set; }
    }
}