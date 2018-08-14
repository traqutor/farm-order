﻿using FarmOrder.Data.Entities;
using FarmOrder.Models;
using FarmOrder.Models.Users;
using FarmOrder.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmOrder.Controllers
{
    //[Authorize(Roles = "Admin, CustomerAdmin")]
    public class UsersManagementController : ApiController
    {
        private readonly UserManagementService _service;

        public UsersManagementController()
        {
            _service = new UserManagementService();
        }

        public SearchResults<UserListEntryViewModel> Get(int page, int? customerId, int? siteId)
        {
            if (User.IsInRole("Admin"))
                return _service.GetUsers(User.Identity.GetUserId(), true, page, customerId, siteId);
            else  
                return _service.GetUsers(User.Identity.GetUserId(), false, page, null, null);
        }

        public UserListEntryViewModel Post([FromBody]UserCreateModel model)
        {
            if (User.IsInRole("Admin"))
                return _service.Add(User.Identity.GetUserId(), true, model);
            else
                return _service.Add(User.Identity.GetUserId(), false, model);
        }
    }
}
