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
using FarmOrder.Data.Entities.Farms;

namespace FarmOrder.Services
{
    public class UserManagementService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserListEntryViewModel Get(string userId, bool isAdmin, string searchedUserId)
        {
            var searchedUser = _context.Users.SingleOrDefault(u => u.Id == searchedUserId);

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                if(loggedUser.CustomerId != searchedUser.CustomerId)
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var possibleRoles = _roleManager.Roles.ToList();
            return new UserListEntryViewModel(searchedUser, possibleRoles);
        }

        public UserManagementService()
        {
            _context = FarmOrderDBContext.Create();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new UserManager<User>(new UserStore<User>(_context));
        }

        public SearchResults<UserListEntryViewModel> GetUsers(string userId, bool isAdmin, int? page, int? customerId, int? siteId)
        {
            var query = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var possibleRoles = _roleManager.Roles.ToList();
            var adminRole = possibleRoles.SingleOrDefault(r => r.Name == UserSystemRoles.Admin);

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerId == loggedUser.CustomerId && !u.Roles.Any(r => r.RoleId == adminRole.Id)); //removing the administrator from here
            }
            else
            {
                if(customerId.HasValue && customerId != 0)
                    query = query.Where(u => u.CustomerId == customerId.Value);
            }

            int totalCount = query.Count();

            if(page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

            
            return new SearchResults<UserListEntryViewModel>{
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new UserListEntryViewModel(el, possibleRoles)).ToList()
            };
        }

        public UserListEntryViewModel Add(string userId, bool isAdmin, UserCreateModel model, HttpRequestMessage request)
        {
            Customer selectedCustomer = _context.Customers.Include(c => c.CustomerSites).Include("CustomerSites.Farms").SingleOrDefault(c => c.Id == model.Customer.Id);
            IdentityRole role = _roleManager.FindById(model.RoleId);
            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                selectedCustomer = _context.Customers.Include(c => c.CustomerSites).SingleOrDefault(c => c.Id == loggedUser.CustomerId);

                if (role.Name == UserSystemRoles.Admin)
                    errors.Add("Normal user can not assign Admin role.");
            }

            if(role == null)
                errors.Add("Role can not be empty.");

            if (selectedCustomer == null)
                errors.Add("Customer can not be empty.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            var user = new User();
            user.UserName = model.UserName;
            user.Email = model.UserName;
            user.EmailConfirmed = true;
            user.CustomerId = selectedCustomer.Id;
            user.CreationDate = DateTime.UtcNow;
            user.ModificationDate = DateTime.UtcNow;
            user.CreatedById = userId;

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

                if (model.Farms != null)
                {
                    int[] farmsIds = model.Farms.Select(f => f.Id).ToArray();

                    List<Farm> farms = selectedCustomer.CustomerSites.SelectMany(cs => cs.Farms.Where(f => farmsIds.Contains(f.Id))).ToList();
                    farms.ForEach(farm =>
                    {
                        _context.FarmUsers.Add(new FarmUser { User = user, FarmId = farm.Id });
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

            var userToReturn = _context.Users.Include("FarmUsers.Farm").Include(u => u.Roles).SingleOrDefault(u => u.Id == user.Id);
            var possibleRoles = _roleManager.Roles.ToList();
            return new UserListEntryViewModel(userToReturn, possibleRoles);
        }

        public UserListEntryViewModel UpdateUserPassword(string userId, bool isAdmin, string id, UserPasswordEditModel model, HttpRequestMessage request)
        {
            User userToUpdate = _context.Users.Include(c => c.CustomerSiteUser).SingleOrDefault(u => u.Id == id);
            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

                if (userToUpdate.CustomerId != userToUpdate.CustomerId)
                    errors.Add("Can not update user that belong to different customer.");
            }
            if(userToUpdate == null)
                errors.Add("User not found.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            _userManager.RemovePassword(userToUpdate.Id);
            _userManager.AddPassword(userToUpdate.Id, model.Password);


            var possibleRoles = _roleManager.Roles.ToList();
            var userToReturn = _context.Users.SingleOrDefault(u => u.Id == userToUpdate.Id);
            return new UserListEntryViewModel(userToReturn, possibleRoles);
        }

        public void Delete(string userId, bool isAdmin, string userToDeleteId, HttpRequestMessage request)
        {
            User userToDelete = _context.Users.SingleOrDefault(u => u.Id == userToDeleteId);
            List<string> errors = new List<string>();

            if(_userManager.IsInRole(userToDelete.Id, UserSystemRoles.Admin))
            {
                errors.Add("Cant remove administrator.");
            }

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                if (userToDelete.CustomerId != loggedUser.CustomerId)
                    throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, new List<string> { "Can not delete User of a different customer." }));
            }

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            _context.FarmUsers.RemoveRange(userToDelete.FarmUsers);
            _context.CustomerSiteUsers.RemoveRange(userToDelete.CustomerSiteUser);
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }

        public UserListEntryViewModel Update(string userId, bool isAdmin, string id, UserEditModel model, HttpRequestMessage request)
        {
            User userToUpdate = _context.Users.Include(c => c.CustomerSiteUser).SingleOrDefault(u => u.Id == id);
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
            if (role == null)
                errors.Add("Role can not be empty.");

            if (userToUpdate.UserName != model.UserName)
            {
                var existsUsername = _context.Users.Any(u => u.UserName == model.UserName);
                if(existsUsername)
                    errors.Add("Username is already in Use.");
            }

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            var roles = _userManager.GetRoles(userToUpdate.Id);
            _userManager.RemoveFromRoles(userToUpdate.Id, roles.ToArray());
            var result = _userManager.AddToRole(userToUpdate.Id, role.Name);

            if (model.Customer?.CustomerSites != null)
            {
                int[] sitesIds = model.Customer.CustomerSites.Select(s => s.Id).ToArray();
                List<CustomerSite> sites = selectedCustomer.CustomerSites.Where(cs => sitesIds.Contains(cs.Id)).ToList();

                List<CustomerSiteUser> bindingsToRemove = new List<CustomerSiteUser>();
                List<CustomerSiteUser> bindingsToAdd = new List<CustomerSiteUser>();

                userToUpdate.CustomerSiteUser.ForEach(el =>
                {
                    if (!sites.Any(s => s.Id == el.CustomerSiteId))
                        bindingsToRemove.Add(el);
                });

                sites.ForEach(el =>
                {
                    if(!userToUpdate.CustomerSiteUser.Any(csu => csu.CustomerSiteId == el.Id))
                        bindingsToAdd.Add(new CustomerSiteUser { UserId = userToUpdate.Id, CustomerSiteId = el.Id });
                });

                _context.CustomerSiteUsers.RemoveRange(bindingsToRemove);
                _context.CustomerSiteUsers.AddRange(bindingsToAdd);
            }

            if (model.Farms != null)
            {
                int[] farmsIds = model.Farms.Select(f => f.Id).ToArray();
                List<Farm> farms = selectedCustomer.CustomerSites.SelectMany(cs => cs.Farms.Where(f => farmsIds.Contains(f.Id))).ToList();

                List<FarmUser> bindingsToRemove = new List<FarmUser>();
                List<FarmUser> bindingsToAdd = new List<FarmUser>();

                userToUpdate.FarmUsers.ForEach(el =>
                {
                    if (!farms.Any(s => s.Id == el.FarmId))
                        bindingsToRemove.Add(el);
                });

                farms.ForEach(el =>
                {
                    if (!userToUpdate.FarmUsers.Any(fu => fu.FarmId == el.Id))
                        bindingsToAdd.Add(new FarmUser { UserId = userToUpdate.Id, FarmId = el.Id });
                });

                _context.FarmUsers.RemoveRange(bindingsToRemove);
                _context.FarmUsers.AddRange(bindingsToAdd);
            }

            if (userToUpdate.UserName != model.UserName)
            {
                userToUpdate.Email = model.UserName;
                userToUpdate.UserName = model.UserName;
            }

            if (userToUpdate.CustomerId != selectedCustomer.Id)
                userToUpdate.CustomerId = selectedCustomer.Id;

            userToUpdate.ModificationDate = DateTime.UtcNow;
            userToUpdate.ModifiedById = userId;

            _context.SaveChanges();
         
            var userToReturn = _context.Users.SingleOrDefault(u => u.Id == userToUpdate.Id);
            var possibleRoles = _roleManager.Roles.ToList();
            return new UserListEntryViewModel(userToReturn, possibleRoles);
        }
    }
}