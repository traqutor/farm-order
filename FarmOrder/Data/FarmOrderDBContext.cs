using FarmOrder.Data.Entities;
using FarmOrder.Data.Entities.Customers;
using FarmOrder.Data.Entities.CustomerSites;
using FarmOrder.Data.Entities.Farms;
using FarmOrder.Data.Entities.Orders;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FarmOrder.Data
{
    public class FarmOrderDBContext : IdentityDbContext<User>
    {
        public FarmOrderDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<FarmOrderDBContext>());
        }

        public static FarmOrderDBContext Create()
        {
            return new FarmOrderDBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerSite> CustomerSites { get; set; }
        public virtual DbSet<CustomerSiteUser> CustomerSiteUsers { get; set; }

        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<FarmUser> FarmUsers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderChangeReason> OrderChangeReasons { get; set; }

    }
}