using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Farms
{
    public class SiloSearchModel
    {
        public int? Page { get; set; }
        public List<ShedListEntryViewModel> Sheds { get; set; } = new List<ShedListEntryViewModel>();
    }
}