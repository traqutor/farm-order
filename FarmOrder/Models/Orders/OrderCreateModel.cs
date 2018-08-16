using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderCreateModel
    {
        public DateTime DeliveryDate { get; set; }

        public int TonesOrdered { get; set; }
        public FarmListEntryViewModel Farm { get; set; }
    }
}