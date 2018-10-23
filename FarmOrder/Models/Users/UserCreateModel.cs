using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
using FarmOrder.Models.CustomerSites;
using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class UserCreateModel : UserPasswordEditModel
    {
        public string Id { get; set; }

        /// <summary>
        /// validated like an email
        /// </summary>
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        /// <summary>
        /// Users customer that he belongs to, and inside the sides that user belongs to
        /// </summary>
        [Required]
        public CustomerListEntryViewModel Customer { get; set; }

        public List<FarmListEntryViewModel> Farms { get; set; } = new List<FarmListEntryViewModel>();

        [Required]
        public string RoleId { get; set; }

        public UserCreateModel()
        {

        }

        public UserCreateModel(User entity)
        {
            Id = entity.Id;
            UserName = entity.UserName;

            if (entity.Customer != null)
            {
                Customer = new CustomerListEntryViewModel(entity.Customer);
                Customer.CustomerSites = entity.CustomerSiteUser.Select(el => new CustomerSiteListEntryViewModel(el.CustomerSite)).ToList();
            }

            RoleId = entity.Roles.FirstOrDefault().RoleId;


        }
    }
}