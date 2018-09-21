using FarmOrder.Data;
using FarmOrder.Data.Entities.Farms;
using FarmOrder.Models;
using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace FarmOrder.Services.Farms
{
    public class SiloService
    {
        private readonly int _pageSize = 20;
        private readonly FarmOrderDBContext _context;

        public SiloService()
        {
            _context = FarmOrderDBContext.Create();
        }

        public SearchResults<SiloListEntryViewModel> GetSiloses(string userId, SiloSearchModel model)
        {
            int[] shedIds = model.Sheds.Select(s => s.Id).ToArray();

            FarmUser farmUser = _context.FarmUsers.SingleOrDefault(fu => fu.UserId == userId && fu.Farm.Sheds.Any(s => shedIds.Contains(s.Id)));

            if (farmUser == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var query = _context.Siloses.Where(s => s.EntityStatus == Data.Entities.EntityStatus.NORMAL && shedIds.Contains(s.ShedId)).OrderBy(s => s.Id).AsQueryable();

            int totalCount = query.Count();

            if(model.Page != null)
                query = query.Take(_pageSize).Skip(_pageSize * model.Page.Value);

            return new SearchResults<SiloListEntryViewModel>
            {
                ResultsCount = totalCount,
                Results = query.ToList().Select(el => new SiloListEntryViewModel(el)).ToList()
            };
        }
    }
}