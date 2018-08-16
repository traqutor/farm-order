using FarmOrder.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderSearchModel
    {
        public int? Page { get; set; }
        public List<CustomerListEntryViewModel> Customers { get; set; } = new List<CustomerListEntryViewModel>();
        public List<OrderStatusListEntryViewModel> Statuses { get; set; } = new List<OrderStatusListEntryViewModel>();
        public List<OrderChangeReasonListEntryViewModel> ChangeReasons { get; set; } = new List<OrderChangeReasonListEntryViewModel>();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderByAttribute OrderByAttribute { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    public enum OrderByAttribute
    {
        CREATION_DATE,
        DELIVERY_DATE,
        TONS_ORDERED
    }

    public enum SortOrder
    {
        DESCENDING,
        ASCENDING
    }
}