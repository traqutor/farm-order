using FarmOrder.Data.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderChangeReasonListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderChangeReasonListEntryViewModel()
        {

        }

        public OrderChangeReasonListEntryViewModel(OrderChangeReason el)
        {
            Id = el.Id;
            Name = el.Name;
        }
    }
}