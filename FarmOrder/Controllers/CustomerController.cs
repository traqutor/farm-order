using FarmOrder.Models;
using FarmOrder.Models.Customers;
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
    //[Authorize]
    public class CustomerController : ApiController
    {
        private readonly CustomerService _service;

        public CustomerController()
        {
            _service = new CustomerService();
        }

        public SearchResults<CustomerListEntryViewModel> Get(int page)
        {
            if (User.IsInRole("Admin"))
               return _service.GetCustomers(User.Identity.GetUserId(), true, page);
            else
               return _service.GetCustomers(User.Identity.GetUserId(), true, page);
        }
    }
}
