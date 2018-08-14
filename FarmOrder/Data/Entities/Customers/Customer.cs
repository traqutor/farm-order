using FarmOrder.Data.Entities.CustomerSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public EntityStatus EntityStatus { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<CustomerSite> CustomerSites { get; set; }
    }
}