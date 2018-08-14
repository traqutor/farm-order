using FarmOrder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmOrder.Controllers
{
    public class CustomerSiteController : ApiController
    {
        /*
        private readonly CustomerSiteService _service;

        public CustomerSiteController()
        {
            _service = new CustomerSiteService();
        }
        */
        //Returned inside Customer
        //public SearchResults<CustomerSiteListEntryViewModel> Get(int page)
        //{
        //    if (User.IsInRole("Admin"))
        //        return _service.GetCustomerSites(User.Identity.GetUserId(), true, page);
        //    else
        //        return _service.GetCustomerSites(User.Identity.GetUserId(), true, page);
        //}
    }
}
