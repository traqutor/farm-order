using FarmOrder.Data.Entities;
using FarmOrder.Data.Entities.Customers;
using FarmOrder.Data.Entities.CustomerSites;
using FarmOrder.Data.Entities.Farms;
using FarmOrder.Data.Entities.Farms.Sheds;
using FarmOrder.Data.Entities.Orders;
using FarmOrder.Data.Initializers;
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
            Database.SetInitializer(new FarmOrderDBContextInitializer());
        }

        public static FarmOrderDBContext Create()
        {
            return new FarmOrderDBContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasOptional(u => u.CreatedBy).WithMany();
            modelBuilder.Entity<User>().HasOptional(u => u.ModifiedBy).WithMany();

            modelBuilder.Entity<Customer>().HasOptional(u => u.CreatedBy).WithMany();
            modelBuilder.Entity<Customer>().HasOptional(u => u.ModifiedBy).WithMany();
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerSite> CustomerSites { get; set; }
        public virtual DbSet<CustomerSiteUser> CustomerSiteUsers { get; set; }
        public virtual DbSet<Ration> Rations { get; set; }

        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<Shed> Sheds { get; set; }
        public virtual DbSet<Silo> Silos { get; set; }

        public virtual DbSet<FarmRation> FarmsRations { get; set; }
        public virtual DbSet<OrderSilo> OrdersSilos { get; set; }
        public virtual DbSet<FarmUser> FarmUsers { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderChangeReason> OrderChangeReasons { get; set; }
    }
}