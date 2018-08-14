using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
using FarmOrder.Models.CustomerSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class UserListEntryViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public CustomerListEntryViewModel Customer { get; set; }

        public List<CustomerSiteListEntryViewModel> UserSites { get; set; }

        public UserListEntryViewModel()
        {

        }

        public UserListEntryViewModel(User entity)
        {
            Id = entity.Id;
            UserName = entity.UserName;

            if(entity.Customer != null)
                Customer = new CustomerListEntryViewModel(entity.Customer);

            //entity.CustomerSiteUser.foreach (var item in collection)
            //{

            //}
        }
    }
}