using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Farms.Sheds;
using FarmOrder.Data.Entities.Orders;

namespace FarmOrder.Models.Farms
{
    public class SiloListEntryViewModel
    {
        private OrderSilo os;

        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// capacity of the silo - maximum amount that can be stored there
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Amount ordered by the user in specific order - if displayed somehwere else (not in order context) its 0 
        /// </summary>
        public int Amount { get; set; }

        public int ShedId { get; set; }

        public SiloListEntryViewModel()
        {

        }

        public SiloListEntryViewModel(Silo entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ShedId = entity.ShedId;
            Capacity = entity.Capacity;
        }

        public SiloListEntryViewModel(OrderSilo os)
        {
            Id = os.Silo.Id;
            Name = os.Silo.Name;
            ShedId = os.Silo.ShedId;
            Amount = os.Amount;
            Capacity = os.Silo.Capacity;
        }
    }
}