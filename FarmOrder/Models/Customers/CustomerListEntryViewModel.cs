﻿using FarmOrder.Data.Entities.Customers;
using FarmOrder.Models.CustomerSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Customers
{
    public class CustomerListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Logo { get; set; }
        public string CssFilePath { get; set; }

        public List<CustomerSiteListEntryViewModel> CustomerSites { get; set; } = new List<CustomerSiteListEntryViewModel>();

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public CustomerListEntryViewModel()
        {

        }

        public CustomerListEntryViewModel(Customer entity)
        {
            Id = entity.Id;
            Name = entity.CompanyName;

            Logo = entity.Logo;
            CssFilePath = entity.CssFilePath;

            CreationDate = entity.CreationDate;
            ModificationDate = entity.ModificationDate;

            entity.CustomerSites.ForEach(el =>
            CustomerSites.Add(new CustomerSiteListEntryViewModel(el))
            );
        }
    }
}