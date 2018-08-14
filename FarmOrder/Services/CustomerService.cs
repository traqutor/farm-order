using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Customers;
using FarmOrder.Data;

namespace FarmOrder.Services
{
    public class CustomerService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public CustomerService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<CustomerListEntryViewModel> GetCustomers(string userId, bool isAdmin, int page)
        {
            var query = _context.Customers.Where(c => c.EntityStatus == Data.Entities.EntityStatus.NORMAL).OrderByDescending(c => c.CreationDate).AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(c => c.Users.Any(u => u.Id == userId));
            }

            int totalCount = query.Count();
            query = query.Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<CustomerListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new CustomerListEntryViewModel(el)).ToList()
            };
        }
    }
}