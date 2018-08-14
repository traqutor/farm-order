using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models.Users;
using FarmOrder.Data;
using FarmOrder.Models;
using System.Data.Entity;
namespace FarmOrder.Services
{
    public class UserManagementService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public UserManagementService()
        {
            _context = FarmOrderDBContext.Create();
        }


        public SearchResults<UserListEntryViewModel> GetUsers(string userId, bool isAdmin, int page, int? customerId, int? siteId)
        {
            var query = _context.Users.OrderBy(u => u.UserName).AsQueryable();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.Include(u => u.Customer).SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerId == loggedUser.CustomerId);
            }

            int totalCount = query.Count();

            query = query.Include(u => u.Claims).Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<UserListEntryViewModel>{
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new UserListEntryViewModel(el)).ToList()
            };
        }
    }
}