using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmOrder.Models.Users
{
    public class UserCreateModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int? CustomerId { get; set; }
        public int[] SitesIds { get; set; }
        public string RoleId { get; set; }
    }
}