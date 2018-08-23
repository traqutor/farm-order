using FarmOrder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Orders;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FarmOrder.Data.Entities.Orders;
using System.Data.Entity;

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
                            query = query.OrderBy(o => o.TonsOrdered);
                            break;
                        default:
                            query = query.OrderByDescending(o => o.TonsOrdered);
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
                var loggedUser = _context.Users.Include(u => u.FarmUsers).SingleOrDefault(u => u.Id == userId);
                var userFarms = loggedUser.FarmUsers.Select(fu => fu.FarmId).ToArray();

                //List<int> userAvalibleSites = loggedUser.CustomerSiteUser.Select(csu => csu.CustomerSiteId).ToList();

                //var avalibleSites = userAvalibleSites;
                //if (sitesSubset.Count > 0)
                    //avalibleSites = avalibleSites.Intersect(sitesSubset).ToList();

                query = query.Where(o => userFarms.Contains(o.Farm.Id));
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

            if(searchModel.StartDate != null)
            {
                query = query.Where(o => o.CreationDate >= searchModel.StartDate);
            }

            if (searchModel.EndDate != null)
            {
                query = query.Where(o => o.CreationDate <= searchModel.EndDate);
            }

            int count = query.Count();

            if (searchModel.Page.HasValue)
            {
                query = query.Skip(_pageSize * searchModel.Page.Value).Take(_pageSize);
            }

            return new SearchResults<OrderListEntryViewModel>
            {
                ResultsCount = count,
                Results = query.ToList().Select(el => new OrderListEntryViewModel(el)).ToList()
            };
        }

        public OrderListEntryViewModel Get(string userId, bool isAdmin, int id)
        {
            Order order = _context.Orders.SingleOrDefault(o => o.Id == id);

            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);
                if (!loggedUser.FarmUsers.Any(fu => fu.FarmId == order.FarmId))
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return new OrderListEntryViewModel(order);
        }

        public OrderListEntryViewModel Update(string userId, bool isAdmin, int id, OrderEditModel model, HttpRequestMessage request)
        {
            var changeReason = _context.OrderChangeReasons.SingleOrDefault(ocr => ocr.Id == model.OrderChangeReason.Id);
            var orderStatus = _context.OrderStatuses.SingleOrDefault(os => os.Id == model.Status.Id);

            List<string> errors = new List<string>();

            if (model.TonsOrdered <= 0)
                errors.Add("Can not order less than 1 tone.");

            if(changeReason == null)
                errors.Add("Order change reason must be provided.");

            if (orderStatus == null)
                errors.Add("Invalid order status.");

            Order oldOrder = _context.Orders.SingleOrDefault(o => o.Id == id);

            if(oldOrder.Status?.Name == "Delivered")
                errors.Add("Can not modify delivered order.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));


            if (!isAdmin)
            {
                var loggedUser = _context.Users.SingleOrDefault(u => u.Id == userId);

                if(!loggedUser.FarmUsers.Any(fu => fu.FarmId == oldOrder.FarmId)) {
                    errors.Add("User does not posses access this farm.");
                    throw new HttpResponseException(request.CreateResponse(HttpStatusCode.Unauthorized, errors));
                }
            }

            oldOrder.ModificationDate = DateTime.UtcNow;
            oldOrder.StatusId = orderStatus.Id;
            oldOrder.ChangeReasonId = changeReason.Id;
            oldOrder.DeliveryDate = model.DeliveryDate;
            oldOrder.TonsOrdered = model.TonsOrdered;

            _context.SaveChanges();

            return new OrderListEntryViewModel(oldOrder);
        }

        public OrderListEntryViewModel Add(string userId, bool isAdmin, OrderCreateModel model, HttpRequestMessage request)
        {
            var selectedFarm = _context.Farms.SingleOrDefault(f => f.Id == model.Farm.Id);

            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.Include(u => u.FarmUsers).SingleOrDefault(u => u.Id == userId);

                bool farmAvalibleForUser = loggedUser.FarmUsers.Any(f => selectedFarm.Id == f.Id);

                if (!farmAvalibleForUser)
                    selectedFarm = null;
            }

            if(model.DeliveryDate < DateTime.UtcNow)
                errors.Add("Can not set the past date.");

            if (model.TonsOrdered <= 0)
                errors.Add("Can not order less than 1 tone.");

            if (selectedFarm == null)
                errors.Add("Farm unavalibe select correct farm.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            var defaultStatus = _context.OrderStatuses.SingleOrDefault(s => s.Name == "Open");

            Order order = new Order()
            {
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                StatusId = defaultStatus.Id,
                TonsOrdered = model.TonsOrdered,
                DeliveryDate = model.DeliveryDate,
                FarmId = selectedFarm.Id
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return new OrderListEntryViewModel(order);
        }
    }
}