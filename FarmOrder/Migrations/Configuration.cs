namespace FarmOrder.Migrations
{
    using FarmOrder.Data.Entities;
    using FarmOrder.Data.Entities.Customers;
    using FarmOrder.Data.Entities.CustomerSites;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FarmOrder.Data.FarmOrderDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FarmOrder.Data.FarmOrderDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<User>(new UserStore<User>(context));

            #region Roles creation


            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("CustomerAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "CustomerAdmin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }

            #endregion

         


            #region customers and sites creation
            
            Customer customer1 = new Customer()
            {
                Id = 1,
                CompanyName = "Profisol",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Customer customer2 = new Customer()
            {
                Id = 2,
                CompanyName = "Familyfoods",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Customer customer3 = new Customer()
            {
                Id = 3,
                CompanyName = "Wholenature",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            context.Customers.AddOrUpdate(customer1);
            context.Customers.AddOrUpdate(customer2);
            context.Customers.AddOrUpdate(customer3);

            CustomerSite site1 = new CustomerSite()
            {
                Id = 1,
                SiteName = "Melbourne",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = customer1.Id
            };

            CustomerSite site2 = new CustomerSite()
            {
                Id = 2,
                SiteName = "Home",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = customer2.Id
            };

            CustomerSite site3 = new CustomerSite()
            {
                Id = 3,
                SiteName = "Home",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = customer3.Id
            };

            CustomerSite site4 = new CustomerSite()
            {
                Id = 4,
                SiteName = "Brisbane",
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = customer1.Id
            };

            context.CustomerSites.AddOrUpdate(site1);
            context.CustomerSites.AddOrUpdate(site2);
            context.CustomerSites.AddOrUpdate(site3);
            context.CustomerSites.AddOrUpdate(site4);
            
            #endregion

            #region SystemAdmin creation

            var user = new User();
            user.UserName = "sysadmin";
            //user.Email = "sysadmin@gmail.com";
            user.EmailConfirmed = true;

            string userPWD = "Profisol";

            var chkUser = UserManager.Create(user, userPWD);

            if (chkUser.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, "Admin");
            }

            #endregion

            #region users creation

            var user2 = new User();
            user2.UserName = "FamilyFoodsAdmin";
            //user.Email = "sysadmin@gmail.com";
            user2.EmailConfirmed = true;

            string user2PWD = "Password";

            var chkUser2 = UserManager.Create(user2, user2PWD);

            if (chkUser2.Succeeded)
            {
                var result = UserManager.AddToRole(user2.Id, "CustomerAdmin");
            }


            var user3 = new User();
            user3.UserName = "WholenatureAdmin";
            //user.Email = "sysadmin@gmail.com";
            user3.EmailConfirmed = true;

            string user3PWD = "Password";

            var chkUser3 = UserManager.Create(user3, user3PWD);

            if (chkUser3.Succeeded)
            {
                var result = UserManager.AddToRole(user3.Id, "CustomerAdmin");
            }

            var user4 = new User();
            user4.UserName = "ProfisolAdmin";
            user4.EmailConfirmed = true;

            string user4PWD = "Password";

            var chkUser4 = UserManager.Create(user4, user3PWD);

            if (chkUser4.Succeeded)
            {
                var result = UserManager.AddToRole(user4.Id, "CustomerAdmin");
            }

            var user5 = new User();
            user5.UserName = "FamilyfoodsBrisbane";
            user5.EmailConfirmed = true;

            string user5PWD = "Password";

            var chkUser5 = UserManager.Create(user5, user3PWD);

            if (chkUser5.Succeeded)
            {
                var result = UserManager.AddToRole(user5.Id, "CustomerAdmin");
            }

            #endregion

            #region binding users to the sites

            /* CustomerSiteUser csu1 = new CustomerSiteUser
             {
                 Id = 1,
                 UserId = user2.Id,
                 CustomerSiteId = site1.Id
             };

             context.CustomerSiteUsers.AddOrUpdate(csu1);
            */
            #endregion

            context.SaveChanges();
        }
    }
}
