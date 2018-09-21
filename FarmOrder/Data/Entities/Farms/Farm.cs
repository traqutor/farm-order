using FarmOrder.Data.Entities.CustomerSites;
using FarmOrder.Data.Entities.Farms.Sheds;
using FarmOrder.Data.Entities.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Farms
{
    public class Farm
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public virtual List<Shed> Sheds { get; set; } = new List<Shed>();

        public int CustomerSiteId { get; set; }
        [ForeignKey("CustomerSiteId")]
        public virtual CustomerSite CustomerSite { get; set; }

        public EntityStatus EntityStatus { get; set; }
    }
}