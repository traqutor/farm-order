using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Data.Entities.Orders;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FarmOrder.Models.Orders
{
    public class OrderStatusListEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderStatusListEntryViewModel()
        {

        }

        public OrderStatusListEntryViewModel(OrderStatus el)
        {
            Id = el.Id;
            Name = el.Name;
        }
    }
}