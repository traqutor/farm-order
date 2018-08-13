using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.CustomerSites
{
    public class CustomerSiteUser
    {
        public int Id { get; set; }

        public int CustomerSiteId { get; set; }
        [ForeignKey("CustomerSiteId")]
        public virtual CustomerSite CustomerSite { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}