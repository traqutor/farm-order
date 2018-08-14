using FarmOrder.Data.Entities.CustomerSites;
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

        public int CustomerSiteId { get; set; }
        [ForeignKey("CustomerSiteId")]
        public virtual CustomerSite CustomerSite { get; set; }
    }
}