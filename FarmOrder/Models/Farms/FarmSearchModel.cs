using FarmOrder.Models.CustomerSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Farms
{
    public class FarmSearchModel
    {
        public int? Page { get; set; }
        public List<CustomerSiteListEntryViewModel> CustomerSites { get; set; } = new List<CustomerSiteListEntryViewModel>();
    }
}