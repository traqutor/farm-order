using FarmOrder.Models.CustomerSites;
using FarmOrder.Models.Farms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmOrder.Models.Orders.MultipleOrders
{
    public class MultipleOrderModel
    {

        public bool IsEmergency { get; set; }
        public string Notes { get; set; }
        [Required]
        public RationListEntryViewModel Ration { get; set; }
        [Required]
        public FarmListEntryViewModel Farm { get; set; }
        [Required]
        public List<SiloWithMultipleAmountModel> Silos { get; set; }
    }
}