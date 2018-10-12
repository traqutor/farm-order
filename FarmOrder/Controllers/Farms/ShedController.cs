using FarmOrder.Models;
using FarmOrder.Models.Farms;
using FarmOrder.Services.Farms;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmOrder.Controllers.Farms
{
    [Authorize]
    public class ShedController : ApiController
    {
        private readonly ShedService _service;

        public ShedController()
        {
            _service = new ShedService();
        }

        [HttpGet]
        public SearchResults<ShedListEntryViewModel> GetShedsForOrderCreation(int farmId, int page = 0)
        {
            if(User.IsInRole("Admin"))
                return _service.GetSheds(User.Identity.GetUserId(), true, farmId, page);

            return _service.GetSheds(User.Identity.GetUserId(), false, farmId, page);
        }
    }
}
