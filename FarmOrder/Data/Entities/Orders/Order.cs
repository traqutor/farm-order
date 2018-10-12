using FarmOrder.Data.Entities.CustomerSites;
using FarmOrder.Data.Entities.Farms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Orders
{
    public class Order
    {
        public int Id { get; set; }

     
        public DateTime DeliveryDate { get; set; }

        public virtual List<OrderSilo> Silos { get; set; } = new List<OrderSilo>();

        public int TonsOrdered { get; set; }

        public string Notes { get; set; }

        public int RationId { get; set; }
        [ForeignKey("RationId")]
        public virtual Ration Ration { get; set; }

        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        public virtual Farm Farm { get; set; }

        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual OrderStatus Status { get; set; }

        public int? ChangeReasonId { get; set; }
        [ForeignKey("ChangeReasonId")]
        public virtual OrderChangeReason ChangeReason { get; set; }



        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        public string ModifiedById { get; set; }
        [ForeignKey("ModifiedById")]
        public User ModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public EntityStatus EntityStatus { get; set; }
    }
}