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
        public int CompanyName { get; set; }

        public virtual List<CustomerSite> CustomerSites { get; set; }
    }
}