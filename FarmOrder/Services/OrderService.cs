using FarmOrder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Orders;

namespace FarmOrder.Services
{
    public class OrderService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public OrderService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<OrderListEntryViewModel> GetOrders(string userId, bool isAdmin, OrderSearchModel searchModel)
        {
            var query = _context.Orders.AsQueryable();

            switch (searchModel.OrderByAttribute)
            {
                case OrderByAttribute.DELIVERY_DATE:
                    switch (searchModel.SortOrder)
                    {
                        case SortOrder.ASCENDING:
                            query = query.OrderBy(o => o.DeliveryDate);
                            break;
                        default:
                            query = query.OrderByDescending(o => o.DeliveryDate);
                            break;
                    }
                    break;
                case OrderByAttribute.TONS_ORDERED:
                    switch (searchModel.SortOrder)
                    {
                        case SortOrder.ASCENDING:
                            query = query.OrderBy(o => o.TonesOrdered);
                            break;
                        default:
                            query = query.OrderByDescending(o => o.TonesOrdered);
                            break;
                    }
                    break;
                default: //sorting by creation date as default
                    switch (searchModel.SortOrder)
                    {
                        case SortOrder.ASCENDING:
                            query = query.OrderBy(o => o.CreationDate);
                            break;
                        default:
                            query = query.OrderByDescending(o => o.CreationDate);
                            break;
                    }
                    break;
            }

            List<int> sitesSubset = new List<int>();

            foreach (var customer in searchModel.Customers)
            {
                foreach (var site in customer.CustomerSites)
                {
                    sitesSubset.Add(site.Id);
                }
            }

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                var customerFarms = _context.Farms.Where(f => f.CustomerSite.CustomerId == loggedUser.CustomerId);

                List<int> userAvalibleSites = loggedUser.CustomerSiteUser.Select(csu => csu.CustomerSiteId).ToList();

                var avalibleSites = userAvalibleSites;
                if (sitesSubset.Count > 0)
                    avalibleSites = avalibleSites.Intersect(sitesSubset).ToList();

                var avalibleSitesArr = avalibleSites.ToArray();
                query = query.Where(o => avalibleSitesArr.Contains(o.Farm.CustomerSiteId));
            }
            else
            {
                if (sitesSubset.Count > 0)
                {
                    var sitesSubsetArr = sitesSubset.ToArray();
                    query = query.Where(o => sitesSubsetArr.Contains(o.Farm.CustomerSiteId));
                }
            }

            if(searchModel.Statuses.Count > 0)
            {
                int[] statsuesIds = searchModel.Statuses.Select(s => s.Id).ToArray();
                query = query.Where(o => statsuesIds.Contains(o.StatusId));
            }

            /*
            if (searchModel.ChangeReasons.Count > 0)
            {
                int?[] changeReasonsIds = searchModel.ChangeReasons.Select(s => (int?)s.Id).ToArray<int?>();
                query = query.Where(o => changeReasonsIds.Contains(o.ChangeReasonId));
            }
            */

            return new SearchResults<OrderListEntryViewModel>
            {
                ResultsCount = query.Count(),
                Results = query.ToList().Select(el => new OrderListEntryViewModel(el)).ToList()
            };
        }

        public OrderListEntryViewModel Add(string v1, bool v2, OrderCreateModel model)
        {
            throw new NotImplementedException();
        }
    }
}