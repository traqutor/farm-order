using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Farms.Sheds;

namespace FarmOrder.Models.Farms
{
    public class SiloListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ShedId { get; set; }

        public SiloListEntryViewModel()
        {

        }

        public SiloListEntryViewModel(Silo entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ShedId = entity.ShedId;
        }
    }
}