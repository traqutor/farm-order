using FarmOrder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmOrder.Models;
using FarmOrder.Models.Farms;
using FarmOrder.Data.Entities.Farms;
using System.Web.Http;
using System.Net;
using System.Data.Entity;

namespace FarmOrder.Services.Farms
{
    public class ShedService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public ShedService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<ShedListEntryViewModel> GetSheds(string userId, int farmId, int page)
        {
            FarmUser farmUser = _context.FarmUsers.SingleOrDefault(fu => fu.UserId == userId && fu.FarmId == farmId);

            if(farmUser == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var query = _context.Sheds.Include(sh => sh.Siloses).Where(s => s.EntityStatus == Data.Entities.EntityStatus.NORMAL && s.FarmId == farmId).OrderBy(s => s.Id).AsQueryable();

            int totalCount = query.Count();

            query = query.Take(_pageSize).Skip(_pageSize * page);

            return new SearchResults<ShedListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new ShedListEntryViewModel(el)).ToList()
            };
        }
    }
}