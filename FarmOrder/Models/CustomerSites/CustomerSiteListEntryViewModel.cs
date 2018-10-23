using FarmOrder.Data.Entities.CustomerSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.CustomerSites
{
    public class CustomerSiteListEntryViewModel
    {
        public int Id { get; set; }
        public string SiteName { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public CustomerSiteListEntryViewModel()
        {

        }

        public CustomerSiteListEntryViewModel(CustomerSite entity)
        {
            Id = entity.Id;
            SiteName = entity.SiteName;

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;
        }
    }
}