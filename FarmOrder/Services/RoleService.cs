using FarmOrder.Data;
using FarmOrder.Models;
using FarmOrder.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Services
{
    public class RoleService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public RoleService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<RoleListEntryViewModel> GetRoles(int page)
        {
            var query = _context.Roles.OrderByDescending(r => r.Id).Take(_pageSize).Skip(_pageSize * page).ToList();

            return new SearchResults<RoleListEntryViewModel>
            {
                ResultsCount = query.Count(),
                Results = query.ToList().Select(el => new RoleListEntryViewModel(el)).ToList()
            };
        }
    }
}