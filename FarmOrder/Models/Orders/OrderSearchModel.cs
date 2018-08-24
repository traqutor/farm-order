using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;

namespace FarmOrder.Models.Orders
{
    public class OrderSearchModel
    {
        public int? Page { get; set; }
        public List<OrderStatusListEntryViewModel> Statuses { get; set; } = new List<OrderStatusListEntryViewModel>();
        public List<OrderChangeReasonListEntryViewModel> ChangeReasons { get; set; } = new List<OrderChangeReasonListEntryViewModel>();

        public FarmListEntryViewModel Farm { get; set; }

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