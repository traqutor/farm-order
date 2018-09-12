using FarmOrder.Data.Entities;
using FarmOrder.Models;
using FarmOrder.Models.Users;
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
    public class RoleController : ApiController
    {
        private readonly RoleService _service;

        public RoleController()
        {
            _service = new RoleService();
        }


        public SearchResults<RoleListEntryViewModel> GetRoles(int? page = null)
        {
            if(User.IsInRole(UserSystemRoles.Admin))
                return _service.GetRoles(true, page);
            else 
                return _service.GetRoles(false, page);
        }
    }
}
