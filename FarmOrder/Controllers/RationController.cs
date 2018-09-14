using FarmOrder.Models;
using FarmOrder.Models.CustomerSites;
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
    public class RationController : ApiController
    {
        private readonly RationService _service;

        public RationController()
        {
            _service = new RationService();
        }

        public SearchResults<RationListEntryViewModel> GetFarmsForUserCreation(int farmId, int page = 0)
        {
            if (User.IsInRole("Admin"))
                return _service.GetRations(User.Identity.GetUserId(), true, farmId, page);
            else
                return _service.GetRations(User.Identity.GetUserId(), false, farmId, page);
        }
    }
}
