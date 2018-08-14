using FarmOrder.Models;
using FarmOrder.Models.Users;
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
    [Authorize(Roles = "Admin, CustomerAdmin")]
    public class UsersManagementController : ApiController
    {
        private readonly UserManagementService _service;

        public UsersManagementController()
        {
            _service = new UserManagementService();
        }

        public SearchResults<UserListEntryViewModel> Get(int page, int customerId, int siteId)
        {
            if (User.IsInRole("Admin"))
                return _service.GetUsers(User.Identity.GetUserId(), true, page, customerId, siteId);
            else if (User.IsInRole("CustomerAdmin"))
                return _service.GetUsers(User.Identity.GetUserId(), true, page, null, null);
            else
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}
