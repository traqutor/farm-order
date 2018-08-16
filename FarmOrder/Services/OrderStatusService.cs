using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Orders;
using FarmOrder.Data;

namespace FarmOrder.Services
{
    public class OrderStatusService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public OrderStatusService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<OrderStatusListEntryViewModel> GetOrderStatuses(int? page)
        {
            var query = _context.OrderStatuses.OrderByDescending(r => r.Id).AsQueryable();
            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

            return new SearchResults<OrderStatusListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new OrderStatusListEntryViewModel(el)).ToList()
            };
        }

    
    }
}