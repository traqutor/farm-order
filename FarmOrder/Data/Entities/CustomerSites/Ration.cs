using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.CustomerSites
{
    public class Ration
    {
        public int Id { get; set; }

        public int CustomerSiteId { get; set; }
        [ForeignKey("CustomerSiteId")]
        public virtual CustomerSite CustomerSite { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}