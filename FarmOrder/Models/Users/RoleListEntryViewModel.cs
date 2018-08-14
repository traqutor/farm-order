using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class RoleListEntryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public RoleListEntryViewModel()
        {

        }

        public RoleListEntryViewModel(IdentityRole entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}