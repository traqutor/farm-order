using FarmOrder.Models.CustomerSites;
using FarmOrder.Models.Farms;
using FarmOrder.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders.MultipleOrders
{
    public class SiloWithMultipleAmountModel
    {
        public int? Id { get; set; }
        /// <summary>
        /// Amount ordered by the user in specific order - if displayed somehwere else (not in order context) its 0 
        /// </summary>
        public List<DateAmountModel> DateAmount { get; set; }
    }
}