using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.CustomerSites;

namespace FarmOrder.Models.CustomerSites
{
    public class RationListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public RationListEntryViewModel()
        {

        }

        public RationListEntryViewModel(Ration el)
        {
            Id = el.Id;
            Name = el.Name;
            Description = el.Description;
        }
    }
}