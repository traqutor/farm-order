using FarmOrder.Data.Entities;
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
    [Authorize(Roles = "Admin, CustomerAdmin")]
    public class UsersManagementController : ApiController
    {
        private readonly UserManagementService _service;

        public UsersManagementController()
        {
            _service = new UserManagementService();
        }

        /// <summary>
        /// page ignored, pagination done in front end
        /// </summary>
        /// <param name="page"></param>
        /// <param name="customerId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public SearchResults<UserListEntryViewModel> Get(int? page = null, int? customerId = null, int? siteId = null)
        {
            if (User.IsInRole("Admin"))
                return _service.GetUsers(User.Identity.GetUserId(), true, page, customerId, siteId);
            else  
                return _service.GetUsers(User.Identity.GetUserId(), false, page, null, null);
        }

        public UserListEntryViewModel Post([FromBody]UserCreateModel model)
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

        public UserListEntryViewModel Put(string id, [FromBody]UserCreateModel model)
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

        public void Delete(string id)
        {
            if (User.IsInRole("Admin"))
                _service.Delete(User.Identity.GetUserId(), true, id, Request);
            else
                _service.Delete(User.Identity.GetUserId(), false, id, Request);
        }
    }
}
