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

            /*foreach (var customer in searchModel.Customers)
            {
                foreach (var site in customer.CustomerSites)
                {
                    sitesSubset.Add(site.Id);
                }
            }*/

            if (!isAdmin)
            {
                var loggedUser = _context.Users.Include(u => u.FarmUsers).SingleOrDefault(u => u.Id == userId);
                var userFarms = loggedUser.FarmUsers.Select(fu => fu.FarmId).ToArray();


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

            if(searchModel.Farm?.Id != null)
            {
                var selectedFarm = _context.Farms.SingleOrDefault(f => f.Id == searchModel.Farm.Id);
                query = query.Where(o => o.FarmId == selectedFarm.Id);
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
                query = query.Where(o => o.DeliveryDate >= searchModel.StartDate);
            }

            if (searchModel.EndDate != null)
            {
                query = query.Where(o => o.DeliveryDate <= searchModel.EndDate);
            }

            int count = query.Count();

            if (searchModel.Page.HasValue)
            {
                query = query.Skip(_pageSize * searchModel.Page.Value).Take(_pageSize);
            }

            return new SearchResults<OrderListEntryViewModel>
            {
                ResultsCount = count,
                Results = query.Include("Silos.Silo").ToList().Select(el => new OrderListEntryViewModel(el)).ToList()
            };
        }

        public OrderListEntryViewModel Get(string userId, bool isAdmin, int id)
        {
            Order order = _context.Orders.Include("Silos.Silo").SingleOrDefault(o => o.Id == id);

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
            var selectedRation = _context.Rations.SingleOrDefault(r => r.Id == model.Ration.Id && r.CustomerSite.Farms.Any(f => f.Id == model.Farm.Id));

            int[] silosesIds = model.Silos.Select(s => s.Id).ToArray();
            var selectedSiloses = _context.Silos.Where(s => silosesIds.Contains(s.Id) && model.Farm.Id == s.Shed.FarmId).ToList();

            List<string> errors = new List<string>();

            if (model.TonsOrdered <= 0)
                errors.Add("Can not order less than 1 tone.");

            if(changeReason == null)
                errors.Add("Order change reason must be provided.");

            if (orderStatus == null)
                errors.Add("Invalid order status.");

            Order oldOrder = _context.Orders.Include("Silos.Silo").SingleOrDefault(o => o.Id == id);

            if (oldOrder.Status?.Name == "Delivered")
                errors.Add("Can not modify delivered order.");

            if (selectedRation == null)
                errors.Add("Ration unavalibe select correct ration.");

            if (selectedSiloses == null || selectedSiloses.Count() <= 0)
                errors.Add("Atleast one silo needs to be selected.");

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
            oldOrder.ModifiedById = userId;
            oldOrder.StatusId = orderStatus.Id;
            oldOrder.ChangeReasonId = changeReason.Id;
            oldOrder.DeliveryDate = model.DeliveryDate;
            oldOrder.TonsOrdered = model.TonsOrdered;
            oldOrder.RationId = selectedRation.Id;

            List<OrderSilo> bindingsToRemove = new List<OrderSilo>();
            List<OrderSilo> bindingsToAdd = new List<OrderSilo>();

            oldOrder.Silos.ForEach(el =>
            {
                if (!selectedSiloses.Any(s => s.Id == el.SiloId))
                    bindingsToRemove.Add(el);
            });

            model.Silos.ForEach(el =>
            {
                if (!oldOrder.Silos.Any(s => s.SiloId == el.Id)) { 
                    bindingsToAdd.Add(new OrderSilo {
                        SiloId = el.Id,
                        OrderId = oldOrder.Id,
                        Amount = el.Amount,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow,
                        EntityStatus = Data.Entities.EntityStatus.NORMAL
                    });
                }
                else
                {
                    OrderSilo os = oldOrder.Silos.SingleOrDefault(s => s.SiloId == el.Id);
                    os.Amount = el.Amount;
                }
            });

            _context.OrdersSilos.RemoveRange(bindingsToRemove);
            _context.OrdersSilos.AddRange(bindingsToAdd);
           
            _context.SaveChanges();

            return new OrderListEntryViewModel(oldOrder);
        }

        public OrderListEntryViewModel Add(string userId, bool isAdmin, OrderCreateModel model, HttpRequestMessage request)
        {
            var selectedFarm = _context.Farms.SingleOrDefault(f => f.Id == model.Farm.Id);
            var selectedRation = _context.Rations.SingleOrDefault(r => r.Id == model.Ration.Id && r.CustomerSite.Farms.Any(f => f.Id == model.Farm.Id));

            int[] silosesIds = model.Silos.Select(s => s.Id).ToArray();
            var selectedSiloses = _context.Silos.Where(s => silosesIds.Contains(s.Id) && model.Farm.Id == s.Shed.FarmId);

            List<string> errors = new List<string>();

            if (!isAdmin)
            {
                var loggedUser = _context.Users.Include(u => u.FarmUsers).SingleOrDefault(u => u.Id == userId);

                bool farmAvalibleForUser = loggedUser.FarmUsers.Any(f => selectedFarm.Id == f.FarmId);

                if (!farmAvalibleForUser)
                    selectedFarm = null;
            }

            if(model.DeliveryDate < DateTime.UtcNow)
                errors.Add("Can not set the past date.");

            if (model.TonsOrdered <= 0)
                errors.Add("Can not order less than 1 tone.");

            if (selectedFarm == null)
                errors.Add("Farm unavalibe select correct farm.");

            if(selectedRation == null)
                errors.Add("Ration unavalibe select correct ration.");

            if (selectedSiloses == null || selectedSiloses.Count() <= 0)
                errors.Add("Atleast one silo needs to be selected.");

            if (errors.Count > 0)
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.BadRequest, errors));

            var defaultStatus = _context.OrderStatuses.SingleOrDefault(s => s.Name == "Open");

            Order order = new Order()
            {
                CreatedById = userId,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                StatusId = defaultStatus.Id,
                TonsOrdered = model.TonsOrdered,
                DeliveryDate = model.DeliveryDate,
                FarmId = selectedFarm.Id,
                RationId = selectedRation.Id
            };

            foreach (var silo in model.Silos)
            {
                OrderSilo os = new OrderSilo()
                {
                    Order = order,
                    SiloId = silo.Id,
                    Amount = silo.Amount,
                    EntityStatus = Data.Entities.EntityStatus.NORMAL,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };
                order.Silos.Add(os);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            order = _context.Orders.Include("Silos.Silo").SingleOrDefault(el => order.Id == el.Id);

            return new OrderListEntryViewModel(order);
        }
    }
}