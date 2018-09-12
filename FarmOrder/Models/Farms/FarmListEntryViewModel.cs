using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Farms;
using FarmOrder.Data.Entities.Orders;

namespace FarmOrder.Models.Farms
{
    public class FarmListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FarmListEntryViewModel()
        {

        }

        public FarmListEntryViewModel(Farm entity)
        {
    
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}