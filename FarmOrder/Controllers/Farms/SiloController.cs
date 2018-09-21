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
    public class SiloController : ApiController
    {
        private readonly SiloService _service;

        public SiloController()
        {
            _service = new SiloService();
        }

        [HttpPost]
        public SearchResults<SiloListEntryViewModel> GetShedsForOrderCreation(SiloSearchModel model)
        {
            return _service.GetSiloses(User.Identity.GetUserId(), model);
        }
    }
}
