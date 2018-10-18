using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Orders;
using FarmOrder.Models.CustomerSites;
using FarmOrder.Models.Users;

namespace FarmOrder.Models.Orders
{
    public class OrderListEntryViewModel: OrderCreateModel
    {
        public OrderListEntryViewModel(Order el)
        {
            Id = el.Id;
            TonsOrdered = el.TonsOrdered;
            Notes = el.Notes;
            Ration = new RationListEntryViewModel(el.Ration);
            Farm = new FarmListEntryViewModel(el.Farm);
            CreationDate = el.CreationDate;
            ModificationDate = el.ModificationDate;
            DeliveryDate = el.DeliveryDate;
            Status = new OrderStatusListEntryViewModel(el.Status);

            if (el.CreatedBy != null)
                CreatedBy = new UserListEntryViewModel(el.CreatedBy);
            if (el.ModifiedBy != null)
                ModifiedBy = new UserListEntryViewModel(el.ModifiedBy);

            if (el.ChangeReason != null)
                OrderChangeReason = new OrderChangeReasonListEntryViewModel(el.ChangeReason);
            if (el.Silos != null)
            {
                Silos = new List<SiloListEntryViewModel>();

                foreach (var os in el.Silos)
                {
                    Silos.Add(new SiloListEntryViewModel(os));
                }
            }
        }

        public OrderListEntryViewModel()
        {

        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public UserListEntryViewModel CreatedBy { get; set; }
        public UserListEntryViewModel ModifiedBy { get; set; }

        public OrderStatusListEntryViewModel Status { get; set; }
        public OrderChangeReasonListEntryViewModel OrderChangeReason { get; set; }
    }
}