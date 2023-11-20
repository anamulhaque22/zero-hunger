using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Web;
using zero_hunger.EF.Models;

namespace zero_hunger.EF
{
    public class ZeroHungerContext: DbContext
    {
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<CollectRequest> CollectRequests{ get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Distribution> Distributions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            // Your other model configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}