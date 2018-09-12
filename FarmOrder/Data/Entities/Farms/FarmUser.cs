using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Entities.Farms
{
    public class FarmUser
    {
        public int Id { get; set; }

        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        public virtual Farm Farm { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}