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
    public class OrderStatusController : ApiController
    {
        private readonly OrderStatusService _service;

        public OrderStatusController()
        {
            _service = new OrderStatusService();
        }

        public SearchResults<OrderStatusListEntryViewModel> GetOrderStatuses(int? page = null)
        {
            return _service.GetOrderStatuses(page);
        }
    }
}
