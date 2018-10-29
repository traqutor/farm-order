using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Orders.MultipleOrders
{
    public class DateAmountModel
    {
        public DateTime Date { get; set; }
        public int Amount { get; set; }

        //public override bool Equals(object obj)
        //{
        //    var model = obj as DateAmountModel;
        //    return model != null &&
        //           Date.Year == model.Date.Year && 
        //           Date.Month == model.Date.Month ;
        //}

        //public override int GetHashCode()
        //{
        //    var hashCode = 1493515978;
        //    hashCode = hashCode * -1521134295 + Date.GetHashCode();
        //    return hashCode;
        //}
    }
}