using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Farms;
using FarmOrder.Data;

namespace FarmOrder.Services
{
    public class FarmService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public FarmService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<FarmListEntryViewModel> GetFarms(string userId, bool isAdmin, int? page = null, int? customerId = null)
        {
            var query = _context.Farms.OrderByDescending(r => r.Id).AsQueryable();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerSite.CustomerId == loggedUser.CustomerId);
            }

            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

            return new SearchResults<FarmListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new FarmListEntryViewModel(el)).ToList()
            };
        }
    }
}