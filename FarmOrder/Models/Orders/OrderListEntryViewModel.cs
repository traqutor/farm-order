using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderListEntryViewModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public int TonesOrdered { get; set; }
        public FarmListEntryViewModel Farm { get; set; }

        public OrderStatusListEntryViewModel Status { get; set; }

        public OrderChangeReasonListEntryViewModel OrderChangeReason { get; set; }
    }
}