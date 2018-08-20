using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderEditModel : OrderCreateModel
    {
        [Required]
        public OrderStatusListEntryViewModel Status { get; set; }

        [Required]
        public OrderChangeReasonListEntryViewModel OrderChangeReason { get; set; }
    }
}