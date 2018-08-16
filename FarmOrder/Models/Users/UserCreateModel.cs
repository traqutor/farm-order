using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
using FarmOrder.Models.CustomerSites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class UserCreateModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Users customer that he belongs to, and inside the sides that user belongs to
        /// </summary>
        [Required]
        public CustomerListEntryViewModel Customer { get; set; }

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