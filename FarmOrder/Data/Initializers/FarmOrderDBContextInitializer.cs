using FarmOrder.Data.Entities;
using FarmOrder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Initializers
{
    public class FarmOrderDBContextInitializer : CreateDatabaseIfNotExists<FarmOrderDBContext>
    {
        protected override void Seed(FarmOrderDBContext context)
        {
            base.Seed(context);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<User>(new UserStore<User>(context));

            #region Roles creation


            if (!roleManager.RoleExists("SystemAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "SystemAdmin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("CustomerAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "OrganisationAdmin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "ContentAdmin";
                roleManager.Create(role);
            }

            #endregion

            #region SystemAdmin creation

            var user = new User();
            user.UserName = "sysadmin";
            user.Email = "sysadmin@gmail.com";
            user.EmailConfirmed = true;

            string userPWD = "thinkfast!";

            var chkUser = UserManager.Create(user, userPWD);
            #endregion


            context.SaveChanges();
        }
    }
}