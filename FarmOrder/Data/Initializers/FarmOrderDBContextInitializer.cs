using FarmOrder.Data.Entities;
using FarmOrder.Data.Entities.Customers;
using FarmOrder.Data.Entities.CustomerSites;
using FarmOrder.Data.Entities.Farms;
using FarmOrder.Data.Entities.Farms.Sheds;
using FarmOrder.Data.Entities.Orders;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace FarmOrder.Data.Initializers
{
    public class FarmOrderDBContextInitializer : CreateDatabaseIfNotExists<FarmOrderDBContext>
    {
        protected override void Seed(FarmOrderDBContext context)
        {
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
                SiteName = "Home2",
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

            #region rations creation

            Ration cs1ration1 = new Ration()
            {
                Name = "Wheat Melbourne",
                Description = "100% wheat ration",
                CustomerSite = site1
            };

            Ration cs1ration2 = new Ration()
            {
                Name = "Flour Melbourne",
                Description = "100% flour ration",
                CustomerSite = site1
            };

            Ration cs1ration3 = new Ration()
            {
                Name = "Wheat/Flour Melbourne",
                Description = "50% wheat 50% flour ration",
                CustomerSite = site1
            };

            context.Rations.AddOrUpdate(cs1ration1);
            context.Rations.AddOrUpdate(cs1ration2);
            context.Rations.AddOrUpdate(cs1ration3);

            Ration cs2ration1 = new Ration()
            {
                Name = "Wheat Home",
                Description = "100% wheat ration",
                CustomerSite = site2
            };

            Ration cs2ration2 = new Ration()
            {
                Name = "Flour Home",
                Description = "100% flour ration",
                CustomerSite = site2
            };

            Ration cs2ration3 = new Ration()
            {
                Name = "Wheat/Flour Home",
                Description = "30% wheat 70% flour ration",
                CustomerSite = site2
            };

            context.Rations.AddOrUpdate(cs2ration1);
            context.Rations.AddOrUpdate(cs2ration2);
            context.Rations.AddOrUpdate(cs2ration3);

            Ration cs3ration1 = new Ration()
            {
                Name = "Wheat Home2",
                Description = "100% wheat ration",
                CustomerSite = site3
            };

            Ration cs3ration2 = new Ration()
            {
                Name = "Flour Home2",
                Description = "100% flour ration",
                CustomerSite = site3
            };

            Ration cs3ration3 = new Ration()
            {
                Name = "Wheat/Flour Home2",
                Description = "10% wheat 90% flour ration",
                CustomerSite = site3
            };

            context.Rations.AddOrUpdate(cs3ration1);
            context.Rations.AddOrUpdate(cs3ration2);
            context.Rations.AddOrUpdate(cs3ration3);

            Ration cs4ration1 = new Ration()
            {
                Name = "Wheat Brissbane",
                Description = "100% wheat ration",
                CustomerSite = site4
            };

            Ration cs4ration2 = new Ration()
            {
                Name = "Flour Brissbane",
                Description = "100% flour ration",
                CustomerSite = site4
            };

            Ration cs4ration3 = new Ration()
            {
                Name = "Wheat/Flour Brissbane",
                Description = "60% wheat 40% flour ration",
                CustomerSite = site4
            };

            context.Rations.AddOrUpdate(cs4ration1);
            context.Rations.AddOrUpdate(cs4ration2);
            context.Rations.AddOrUpdate(cs4ration3);


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
            user2.CustomerId = 2;

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
            user3.CustomerId = 3;

            string user3PWD = "Password";

            var chkUser3 = UserManager.Create(user3, user3PWD);

            if (chkUser3.Succeeded)
            {
                var result = UserManager.AddToRole(user3.Id, "CustomerAdmin");
            }

            var user4 = new User();
            user4.UserName = "ProfisolAdmin";
            user4.EmailConfirmed = true;
            user4.CustomerId = 1;

            string user4PWD = "Password";

            var chkUser4 = UserManager.Create(user4, user3PWD);

            if (chkUser4.Succeeded)
            {
                var result = UserManager.AddToRole(user4.Id, "CustomerAdmin");
            }

            var user5 = new User();
            user5.UserName = "FamilyfoodsBrisbane";
            user5.EmailConfirmed = true;
            user5.CustomerId = 2;

            string user5PWD = "Password";

            var chkUser5 = UserManager.Create(user5, user3PWD);

            if (chkUser5.Succeeded)
            {
                var result = UserManager.AddToRole(user5.Id, "CustomerAdmin");
            }

            #endregion

            #region binding users to the sites
            //not adding if already added
            if (context.CustomerSiteUsers.Count() == 0)
            {
                CustomerSiteUser csu1 = new CustomerSiteUser
                {
                    Id = 1,
                    UserId = user2.Id,
                    CustomerSiteId = site1.Id
                };

                CustomerSiteUser csu2 = new CustomerSiteUser
                {
                    Id = 2,
                    UserId = user3.Id,
                    CustomerSiteId = site2.Id
                };

                CustomerSiteUser csu3 = new CustomerSiteUser
                {
                    Id = 3,
                    UserId = user5.Id,
                    CustomerSiteId = site4.Id
                };

                context.CustomerSiteUsers.AddOrUpdate(csu1);
                context.CustomerSiteUsers.AddOrUpdate(csu2);
                context.CustomerSiteUsers.AddOrUpdate(csu3);
            }

            #endregion

            #region order statuses, change reasons

            if (context.OrderStatuses.Count() == 0)
            {
                OrderStatus status1 = new OrderStatus()
                {
                    Id = 1,
                    Name = "Open"
                };

                OrderStatus status2 = new OrderStatus()
                {
                    Id = 2,
                    Name = "Pending"
                };

                OrderStatus status3 = new OrderStatus()
                {
                    Id = 3,
                    Name = "Confirmed"
                };

                OrderStatus status4 = new OrderStatus()
                {
                    Id = 4,
                    Name = "Delivered"
                };

                context.OrderStatuses.Add(status1);
                context.OrderStatuses.Add(status2);
                context.OrderStatuses.Add(status3);
                context.OrderStatuses.Add(status4);
            }

            if (context.OrderChangeReasons.Count() == 0)
            {
                OrderChangeReason changeReason1 = new OrderChangeReason()
                {
                    Id = 1,
                    Name = "Over order"
                };

                OrderChangeReason changeReason2 = new OrderChangeReason()
                {
                    Id = 2,
                    Name = "Late arrival"
                };

                context.OrderChangeReasons.Add(changeReason1);
                context.OrderChangeReasons.Add(changeReason2);
            }

            #endregion

            #region Farms
            if (context.Farms.Count() == 0)
            {
                Farm farm1 = new Farm()
                {
                    Id = 1,
                    Name = "Sunny meadow in Melbourne",
                    CustomerSiteId = 1
                };
                
                Farm farm2 = new Farm()
                {
                    Id = 2,
                    Name = "Kirbiln in Home",
                    CustomerSiteId = 2
                };

                Farm farm3 = new Farm()
                {
                    Id = 3,
                    Name = "MacClaggen in Home",
                    CustomerSiteId = 3
                };

                Farm farm4 = new Farm()
                {
                    Id = 3,
                    Name = "Kukurydza in Brisbane",
                    CustomerSiteId = 4
                };

                List<Farm> farms = new List<Farm>();

                farms.Add(farm1);
                farms.Add(farm2);
                farms.Add(farm3);
                farms.Add(farm4);

                foreach (var farm in farms)
                {
                    #region sheds and siloses

                    Shed shed1Farm1 = new Shed()
                    {
                        Name = $"Shed1 in {farm.Name}",
                        FarmId = farm.Id,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo1Shed1 = new Silo()
                    {
                        Name = $"Silo1 in {shed1Farm1.Name}",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed1Farm1,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo2Shed1 = new Silo()
                    {
                        Name = $"Silo2 in {shed1Farm1.Name}",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed1Farm1,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Shed shed2Farm1 = new Shed()
                    {
                        Name = $"Shed2 in {farm1.Name}",
                        FarmId = farm.Id,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo1Shed2 = new Silo()
                    {
                        Name = $"Silo1 in {shed2Farm1.Name}",
                        Capacity = 20,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo2Shed2 = new Silo()
                    {
                        Name = $"Silo2 in {shed2Farm1.Name}",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo3Shed2 = new Silo()
                    {
                        Name = $"Silo3 in {shed2Farm1.Name}",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };


                    shed1Farm1.Siloses.Add(silo1Shed1);
                    shed1Farm1.Siloses.Add(silo2Shed1);

                    shed2Farm1.Siloses.Add(silo1Shed2);
                    shed2Farm1.Siloses.Add(silo2Shed2);
                    shed2Farm1.Siloses.Add(silo3Shed2);


                    farm.Sheds.Add(shed1Farm1);
                    farm.Sheds.Add(shed2Farm1);
                    #endregion
                }

                context.Farms.Add(farm1);
                context.Farms.Add(farm2);
                context.Farms.Add(farm3);
                context.Farms.Add(farm4);



            }
            #endregion

            context.SaveChanges();
        }
    }
}