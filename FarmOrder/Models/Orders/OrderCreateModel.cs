using FarmOrder.Models.CustomerSites;
using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders
{
    public class OrderCreateModel
    {
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public int TonsOrdered { get; set; }
        [Required]
        public RationListEntryViewModel Ration { get; set; }
        [Required]
        public FarmListEntryViewModel Farm { get; set; }
        [Required]
        public List<SiloListEntryViewModel> Silos { get; set; }
        
    }
}