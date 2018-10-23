using FarmOrder.Models;
using FarmOrder.Models.Orders;
using FarmOrder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmOrder.Controllers
{
    [Authorize]
    public class OrderChangeReasonController : ApiController
    {
        private readonly OrderChangeReasonService _service;

        public OrderChangeReasonController()
        {
            _service = new OrderChangeReasonService();
        }

        public SearchResults<OrderChangeReasonListEntryViewModel> GetOrderStatuses(int? page = null)
        {
            return _service.GetOrderChangeReasons(page);
        }
    }
}
