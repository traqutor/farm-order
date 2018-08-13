using FarmOrder.Data.Entities;
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

           

        }

        //public virtual DbSet<User> Users { get; set; }

    }
}