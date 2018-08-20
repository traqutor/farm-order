using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Farms;
using FarmOrder.Data;
using System.Data.Entity;

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

        public SearchResults<FarmListEntryViewModel> GetFarms(string userId, bool isAdmin, FarmSearchModel model)
        {
            var query = _context.Farms.OrderByDescending(r => r.Id).AsQueryable();

            List<int> sitesSubset = new List<int>();

            foreach (var site in model.CustomerSites)
            {
                sitesSubset.Add(site.Id);
            }

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                query = query.Where(u => u.CustomerSite.CustomerId == loggedUser.CustomerId);

                List<int> userAvalibleSites = _context.CustomerSites.Where(cs => cs.CustomerId == loggedUser.CustomerId).Select(cs => cs.Id).ToList();

                var avalibleSites = userAvalibleSites;
                if (sitesSubset.Count > 0)
                    avalibleSites = avalibleSites.Intersect(sitesSubset).ToList();

                var avalibleSitesArr = avalibleSites.ToArray();
                query = query.Where(f => avalibleSitesArr.Contains(f.CustomerSiteId));
            }
            else
            {
                if (sitesSubset.Count > 0)
                {
                    var sitesSubsetArr = sitesSubset.ToArray();
                    query = query.Where(f => sitesSubsetArr.Contains(f.CustomerSiteId));
                }
            }

            int totalCount = query.Count();

            if (model.Page != null)
                query = query.Take(_pageSize).Skip(_pageSize * model.Page.Value);

            return new SearchResults<FarmListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new FarmListEntryViewModel(el)).ToList()
            };
        }

        public SearchResults<FarmListEntryViewModel> GetUserAssigned(string userId, bool isAdmin, int? page = null)
        {
            var query = _context.FarmUsers.Include(fu => fu.Farm).Where(fu => fu.UserId == userId);

            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

            return new SearchResults<FarmListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new FarmListEntryViewModel(el.Farm)).ToList()
            };
        }
    }
}