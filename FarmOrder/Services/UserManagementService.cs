using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models.Users;
using FarmOrder.Data;
using FarmOrder.Models;

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
            var query = _context.Users;

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
               // query = query.Where(u => u.CustomerId)
            }

            throw new NotImplementedException();
        }
    }
}