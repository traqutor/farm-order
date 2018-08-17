using FarmOrder.Data.Entities;
using FarmOrder.Models.Customers;
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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }

        /// <summary>
        /// Users customer that he belongs to, and inside the sides that user belongs to
        /// </summary>
        [Required]
        public CustomerListEntryViewModel Customer { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}