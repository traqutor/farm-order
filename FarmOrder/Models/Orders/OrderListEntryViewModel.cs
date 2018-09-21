using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Orders;
using FarmOrder.Models.CustomerSites;

namespace FarmOrder.Models.Orders
{
    public class OrderListEntryViewModel: OrderCreateModel
    {
        public OrderListEntryViewModel(Order el)
        {
            Id = el.Id;
            TonsOrdered = el.TonsOrdered;
            Ration = new RationListEntryViewModel(el.Ration);
            Farm = new FarmListEntryViewModel(el.Farm);
            CreationDate = el.CreationDate;
            ModificationDate = el.ModificationDate;
            DeliveryDate = el.DeliveryDate;
            Status = new OrderStatusListEntryViewModel(el.Status);
            if(el.ChangeReason != null)
                OrderChangeReason = new OrderChangeReasonListEntryViewModel(el.ChangeReason);
            if (el.Siloses != null)
            {
                Siloses = new List<SiloListEntryViewModel>();

                foreach (var os in el.Siloses)
                {
                    Siloses.Add(new SiloListEntryViewModel(os.Silo));
                }
            }
        }

        public OrderListEntryViewModel()
        {

        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
       
        public OrderStatusListEntryViewModel Status { get; set; }
        public OrderChangeReasonListEntryViewModel OrderChangeReason { get; set; }
    }
}