using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
using FarmOrder.Models.Farms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class UserEditModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        public List<FarmListEntryViewModel> Farms { get; set; } = new List<FarmListEntryViewModel>();
        /// <summary>
        /// Users customer that he belongs to, and inside the sides that user belongs to
        /// </summary>
        [Required]
        public CustomerListEntryViewModel Customer { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}