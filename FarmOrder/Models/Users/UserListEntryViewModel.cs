using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
using FarmOrder.Models.CustomerSites;
using Microsoft.AspNet.Identity.EntityFramework;
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
    

        /// <summary>
        /// Users customer that he belongs to, and inside the sides that user belongs to
        /// </summary>
        public CustomerListEntryViewModel Customer { get; set; }

        public RoleListEntryViewModel Role { get; set; }
    
        public UserListEntryViewModel()
        {

        }

        public UserListEntryViewModel(User entity, List<IdentityRole> roles)
        {
            Id = entity.Id;
            UserName = entity.UserName;

            if(entity.Customer != null) { 
                Customer = new CustomerListEntryViewModel(entity.Customer);
                Customer.CustomerSites = entity.CustomerSiteUser.Select(el => new CustomerSiteListEntryViewModel(el.CustomerSite)).ToList();
            }
            Role = new RoleListEntryViewModel(roles.SingleOrDefault(r => r.Id == entity.Roles.FirstOrDefault().RoleId));
        }
    }
}