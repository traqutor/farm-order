﻿using FarmOrder.Models;
using FarmOrder.Models.Farms;
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
    public class FarmController : ApiController
    {
        private readonly FarmService _service;

        public FarmController()
        {
            _service = new FarmService();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, CustomerAdmin")]
        public SearchResults<FarmListEntryViewModel> GetFarmsForUserCreation(FarmSearchModel model)
        {
            if (User.IsInRole("Admin"))
                return _service.GetFarms(User.Identity.GetUserId(), true, model);
            else
                return _service.GetFarms(User.Identity.GetUserId(), false, model);
        }

        [Route("api/Farm/GetUserAssignedFarms")]
        public SearchResults<FarmListEntryViewModel> GetUserAssignedFarms(int? page = null)
        {
            if (User.IsInRole("Admin"))
                return _service.GetUserAssigned(User.Identity.GetUserId(), true, page);
            else
                return _service.GetUserAssigned(User.Identity.GetUserId(), false, page);
        }
    }
}
