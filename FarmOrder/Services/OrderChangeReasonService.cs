using FarmOrder.Data;
using FarmOrder.Models;
using FarmOrder.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Services
{
    public class OrderChangeReasonService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public OrderChangeReasonService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<OrderChangeReasonListEntryViewModel> GetOrderChangeReasons(int? page)
        {
            //var query = _context.OrderChangeReasons.OrderByDescending(r => r.Id).Take(_pageSize).Skip(_pageSize * page).ToList();
            var query = _context.OrderChangeReasons.OrderByDescending(r => r.Id).AsQueryable();

            int totalCount = query.Count();

            if (page != null)
                query = query.Take(_pageSize).Skip(_pageSize * page.Value);

            return new SearchResults<OrderChangeReasonListEntryViewModel>
            {
                ResultsCount = query.Count(),
                Results = query.ToList().Select(el => new OrderChangeReasonListEntryViewModel(el)).ToList()
            };
        }
    }
}