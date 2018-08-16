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
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserManagementService()
        {
            _context = FarmOrderDBContext.Create();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new UserManager<User>(new UserStore<User>(_context));
        }


        public SearchResults<UserListEntryViewModel> GetUsers(string userId, bool isAdmin, int page, int? customerId, int? siteId)
        {
            var query = _context.Users.OrderBy(u => u.UserName).AsQueryable();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerId == loggedUser.CustomerId);
            }

            int totalCount = query.Count();

            query = query.Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<UserListEntryViewModel>{
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new UserListEntryViewModel(el)).ToList()
            };
        }

        public UserListEntryViewModel Add(string userId, bool isAdmin, UserCreateModel model, HttpRequestMessage request)
        {
            Customer selectedCustomer = _context.Customers.Include(c => c.CustomerSites).SingleOrDefault(c => c.Id == model.Customer.Id);
            IdentityRole role = _roleManager.FindById(model.RoleId);

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

                if (selectedCustomer.Id != loggedUser.CustomerId)
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                if(role.Name == UserSystemRoles.Admin)
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }

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
    }
}