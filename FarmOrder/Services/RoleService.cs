using FarmOrder.Data;
using FarmOrder.Data.Entities;
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

        public SearchResults<RoleListEntryViewModel> GetRoles(bool isAdmin, int? page)
        {
            
            var query = _context.Roles.OrderByDescending(r => r.Id).AsQueryable();

            if (!isAdmin)
                query = query.Where(role => role.Name != UserSystemRoles.Admin);

            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

           

            return new SearchResults<RoleListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new RoleListEntryViewModel(el)).ToList()
            };
        }
    }
}