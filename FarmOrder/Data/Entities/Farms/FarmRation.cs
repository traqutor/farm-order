using FarmOrder.Data.Entities.CustomerSites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Farms
{
    public class FarmRation
    {
        public int Id { get; set; }

        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        public virtual Farm Farm { get; set; }

        public int RationId { get; set; }
        [ForeignKey("RationId")]
        public virtual Ration Ration { get; set; }
    }
}