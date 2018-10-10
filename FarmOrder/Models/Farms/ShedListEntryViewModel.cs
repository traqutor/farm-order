using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Farms.Sheds;

namespace FarmOrder.Models.Farms
{
    public class ShedListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<SiloListEntryViewModel> Siloses { get; set; }

        public ShedListEntryViewModel()
        {

        }

        public ShedListEntryViewModel(Shed entity)
        {
            Id = entity.Id;
            Name = entity.Name;

            if(entity.Siloses != null)
                Siloses = entity.Siloses.Select(s => new SiloListEntryViewModel(s)).ToList();
        }
    }
}