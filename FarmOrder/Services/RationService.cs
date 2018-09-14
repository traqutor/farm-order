using FarmOrder.Data;
using FarmOrder.Models;
using FarmOrder.Models.CustomerSites;
using System.Linq;

namespace FarmOrder.Services
{
    public class RationService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public RationService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<RationListEntryViewModel> GetRations(string userId, bool isAdmin, int farmId, int page)
        {
            var query = _context.Rations.OrderByDescending(r => r.Id).AsQueryable();
            var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (!isAdmin)
            {
                //making sure the user belongs to the customer 
                query = query.Where(r => r.CustomerSite.CustomerId == loggedUser.CustomerId);
            }

            query = query.Where(r => r.CustomerSite.Farms.Any(f => f.Id == farmId));
         
            int totalCount = query.Count();

             query = query.Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<RationListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new RationListEntryViewModel(el)).ToList()
            };
        }
    }
}