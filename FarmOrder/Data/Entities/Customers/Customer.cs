using FarmOrder.Data.Entities.CustomerSites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<CustomerSite> CustomerSites { get; set; }


        public EntityStatus EntityStatus { get; set; }

        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        public string ModifiedById { get; set; }
        [ForeignKey("ModifiedById")]
        public User ModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}