using FarmOrder.Models;
using FarmOrder.Models.Orders;
using FarmOrder.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmOrder.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private readonly OrderService _service;

        public OrderController()
        {
            _service = new OrderService();
        }

        [HttpPost]
        [Route("api/Order/Search")]
        public SearchResults<OrderListEntryViewModel> Search([FromBody]OrderSearchModel searchModel)
        {
            if (User.IsInRole("Admin"))
                return _service.GetOrders(User.Identity.GetUserId(), true, searchModel);
            else
                return _service.GetOrders(User.Identity.GetUserId(), false, searchModel);
        }

 
        public OrderListEntryViewModel Get(int id)
        {
            if (User.IsInRole("Admin"))
                return _service.Get(User.Identity.GetUserId(), true, id);
            else
                return _service.Get(User.Identity.GetUserId(), false, id);
        }

        public OrderListEntryViewModel Post([FromBody]OrderCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = new
                {
                    message = "Invalid request",
                    errors = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            if (User.IsInRole("Admin"))
                return _service.Add(User.Identity.GetUserId(), true, model, Request);
            else
                return _service.Add(User.Identity.GetUserId(), false, model, Request);
        }

        public OrderListEntryViewModel Put(int id, [FromBody]OrderEditModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = new
                {
                    message = "Invalid request",
                    errors = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            if (User.IsInRole("Admin"))
                return _service.Update(User.Identity.GetUserId(), true, id, model, Request);
            else
                return _service.Update(User.Identity.GetUserId(), false, id, model, Request);
        }

        [Authorize(Roles = "Admin, CustomerAdmin")]
        public void Delete(int id)
        {
            if (User.IsInRole("Admin"))
                _service.Delete(User.Identity.GetUserId(), true, id, Request);
            else
                _service.Delete(User.Identity.GetUserId(), false, id, Request);
        }
    }
}
