using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models.Users;
using FarmOrder.Data;
using FarmOrder.Models;
using System.Data.Entity;
using System.Web.Http;
using FarmOrder.Data.Entities.Customers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using FarmOrder.Data.Entities;
using FarmOrder.Data.Entities.CustomerSites;
using System.Net.Http;
using System.Net;

namespace FarmOrder.Services
{
    public class UserManagementService
    {
        //private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserManagementService()
        {
            _context = FarmOrderDBContext.Create();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new UserManager<User>(new UserStore<User>(_context));
        }


        public SearchResults<UserListEntryViewModel> GetUsers(string userId, bool isAdmin, int? page, int? customerId, int? siteId)
        {
            var query = _context.Users.OrderBy(u => u.UserName).AsQueryable();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerId == loggedUser.CustomerId);
            }
            else
            {
                if(customerId.HasValue && customerId != 0)
                    query = query.Where(u => u.CustomerId == customerId.Value);
            }

            int totalCount = query.Count();

            //query = query.Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<UserListEntryViewModel>{
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new UserListEntryViewModel(el)).ToList()
            };
        }

        public UserListEntryViewModel Add(string userId, bool isAdmin, UserCreateModel model, HttpRequestMessage request)
        {
            Customer selectedCustomer = _context.Customers.Include(c => c.CustomerSites).SingleOrDefault(c => c.Id == model.Customer.Id);
            IdentityRole role = _roleManager.FindById(model.RoleId);
            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

                if (selectedCustomer.Id != loggedUser.CustomerId)
                    errors.Add("User customerId and new user customerId can not be different.");
                if (role.Name == UserSystemRoles.Admin)
                    errors.Add("Normal user can not assign Admin role.");
            }

            if(selectedCustomer == null)
                errors.Add("Customer can not be empty.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));
            var user = new User();
            user.UserName = model.UserName;
            user.EmailConfirmed = true;
            user.CustomerId = selectedCustomer.Id;

            string userPWD = model.Password;

            var chkUser = _userManager.Create(user, userPWD);

            if (chkUser.Succeeded)
            {
                var result = _userManager.AddToRole(user.Id, role.Name);

                if (model.Customer?.CustomerSites != null)
                {
                    int[] sitesIds = model.Customer.CustomerSites.Select(s => s.Id).ToArray();
                    List<CustomerSite> sites = selectedCustomer.CustomerSites.Where(cs => sitesIds.Contains(cs.Id)).ToList();

                    sites.ForEach(site =>
                    {
                        _context.CustomerSiteUsers.Add(new CustomerSiteUser { User = user, CustomerSiteId = site.Id });
                    });
                }
            }
            else
            {
                var error = new
                {
                    message = "Invalid request",
                    errors = chkUser.Errors
                };

                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            _context.SaveChanges();

            var userToReturn = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            return new UserListEntryViewModel(userToReturn);
        }

        public UserListEntryViewModel Update(string userId, bool isAdmin, string id, UserCreateModel model, HttpRequestMessage request)
        {
            User userToUpdate = _context.Users.SingleOrDefault(u => u.Id == id);
            Customer selectedCustomer = _context.Customers.Include(c => c.CustomerSites).SingleOrDefault(c => c.Id == model.Customer.Id);
            IdentityRole role = _roleManager.FindById(model.RoleId);
            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

                if (selectedCustomer.Id != loggedUser.CustomerId)
                    errors.Add("User customerId and new user customerId can not be different.");
                if (role.Name == UserSystemRoles.Admin)
                    errors.Add("Normal user can not assign Admin role.");
                if(userToUpdate?.CustomerId != loggedUser.CustomerId)
                    errors.Add("Can not update user that belong to different customer.");
            }

            if (selectedCustomer == null)
                errors.Add("Customer can not be empty.");
            if (userToUpdate == null)
                errors.Add("Invalid user.");


            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));



           
            /*_userManager.RemoveFromRole(userToUpdate, )
            var result = _userManager.AddToRole(user.Id, role.Name);

            if (model.Customer?.CustomerSites != null)
            {
                int[] sitesIds = model.Customer.CustomerSites.Select(s => s.Id).ToArray();
                List<CustomerSite> sites = selectedCustomer.CustomerSites.Where(cs => sitesIds.Contains(cs.Id)).ToList();

                sites.ForEach(site =>
                {
                    _context.CustomerSiteUsers.Add(new CustomerSiteUser { User = user, CustomerSiteId = site.Id });
                });
            }
           */

            _context.SaveChanges();
         
            var userToReturn = _context.Users.SingleOrDefault(u => u.Id == userToUpdate.Id);

            return new UserListEntryViewModel(userToReturn);
        }
    }
}