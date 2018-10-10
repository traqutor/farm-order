using System.Collections.Generic;
using FarmOrder.Data.Entities.Farms.Sheds;

namespace FarmOrder.Migrations
{
    using FarmOrder.Data.Entities;
    using FarmOrder.Data.Entities.Customers;
    using FarmOrder.Data.Entities.CustomerSites;
    using FarmOrder.Data.Entities.Farms;
    using FarmOrder.Data.Entities.Orders;
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

            //return;

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

            #region SystemAdmin creation

            var sysAdmin = new User();
            sysAdmin.UserName = "sysadmin@gordynpalmer.com.au";
            sysAdmin.Email = "sysadmin@gordynpalmer.com.au";
            sysAdmin.EmailConfirmed = true;
            sysAdmin.CreationDate = DateTime.UtcNow;
            sysAdmin.ModificationDate = DateTime.UtcNow;

            string userPWD = "Password";

            var chkUser = UserManager.Create(sysAdmin, userPWD);

            if (chkUser.Succeeded)
            {
                var result = UserManager.AddToRole(sysAdmin.Id, "Admin");
            }

            #endregion

            #region customers and sites creation

            Customer custProfisol = new Customer()
            {
                Id = 1,
                CompanyName = "Profisol",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Customer custGnP = new Customer()
            {
                Id = 2,
                CompanyName = "GordynPalmer",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Customer custBaiada = new Customer()
            {
                Id = 3,
                CompanyName = "Baiada",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            context.Customers.AddOrUpdate(custProfisol);
            context.Customers.AddOrUpdate(custGnP);
            context.Customers.AddOrUpdate(custBaiada);

            CustomerSite siteProfisolHome = new CustomerSite()
            {
                Id = 1,
                SiteName = "Home",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = custProfisol.Id
            };

            CustomerSite siteGnPHome = new CustomerSite()
            {
                Id = 2,
                SiteName = "Home",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = custGnP.Id
            };

            CustomerSite siteBaiadaHanwood = new CustomerSite()
            {
                Id = 3,
                SiteName = "Hanwood",
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
                CustomerId = custBaiada.Id
            };

            context.CustomerSites.AddOrUpdate(siteProfisolHome);
            context.CustomerSites.AddOrUpdate(siteGnPHome);
            context.CustomerSites.AddOrUpdate(siteBaiadaHanwood);

            #endregion

            #region rations creation

            Ration rationChickenStarter = new Ration()
            {
                Name = "Chicken Starter",
                Description = "Chicken Starter",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Ration rationChickenGrower = new Ration()
            {
                Name = "Chicken Grower",
                Description = "Chicken Grower",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Ration rationChickenFinisher = new Ration()
            {
                Name = "Chicken Finisher",
                Description = "Chicken Finisher",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            context.Rations.AddOrUpdate(rationChickenStarter);
            context.Rations.AddOrUpdate(rationChickenGrower);
            context.Rations.AddOrUpdate(rationChickenFinisher);

            Ration rationDuckStarter = new Ration()
            {
                Name = "Duck Starter",
                Description = "Duck Starter",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Ration rationDuckGrowerA = new Ration()
            {
                Name = "Duck Grower A",
                Description = "Duck Grower A",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Ration rationDuckGrowerB = new Ration()
            {
                Name = "Duck Grower B",
                Description = "Duck Grower B",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            Ration rationDuckFinisher = new Ration()
            {
                Name = "Duck Finisher",
                Description = "Duck Finisher",
                CustomerSite = siteBaiadaHanwood,
                CreatedBy = sysAdmin,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            context.Rations.AddOrUpdate(rationDuckStarter);
            context.Rations.AddOrUpdate(rationDuckGrowerA);
            context.Rations.AddOrUpdate(rationDuckGrowerB);
            context.Rations.AddOrUpdate(rationDuckFinisher);


            #endregion
            
            #region users creation

            var userGnP = new User();
            userGnP.UserName = "info@gordynpalmer.com.au";
            userGnP.Email = "info@gordynpalmer.com.au";
            userGnP.EmailConfirmed = true;
            userGnP.CustomerId = custGnP.Id;
            userGnP.CreatedBy = sysAdmin;
            userGnP.CreationDate = DateTime.UtcNow;
            userGnP.ModificationDate = DateTime.UtcNow;

            string gnpPassword = "Password";

            var chkUser2 = UserManager.Create(userGnP, gnpPassword);

            if (chkUser2.Succeeded)
            {
                var result = UserManager.AddToRole(userGnP.Id, "Admin");
            }


            var userBaiada = new User();
            userBaiada.UserName = "admin@baiada.com.au";
            userBaiada.Email = "admin@baiada.com.au";
            userBaiada.EmailConfirmed = true;
            userBaiada.CustomerId = custBaiada.Id;
            userBaiada.CreatedBy = sysAdmin;
            userBaiada.CreationDate = DateTime.UtcNow;
            userBaiada.ModificationDate = DateTime.UtcNow;

            string baiadaPassword = "Password";

            var chkUser3 = UserManager.Create(userBaiada, baiadaPassword);

            if (chkUser3.Succeeded)
            {
                var result = UserManager.AddToRole(userBaiada.Id, "CustomerAdmin");
            }

            #endregion

            #region binding users to the sites
            //not adding if already added
            if (context.CustomerSiteUsers.Count() == 0)
            {
                CustomerSiteUser csu1 = new CustomerSiteUser
                {
                    Id = 1,
                    UserId = userGnP.Id,
                    CustomerSiteId = siteGnPHome.Id
                };

                CustomerSiteUser csu2 = new CustomerSiteUser
                {
                    Id = 2,
                    UserId = userBaiada.Id,
                    CustomerSiteId = siteBaiadaHanwood.Id
                };

                context.CustomerSiteUsers.AddOrUpdate(csu1);
                context.CustomerSiteUsers.AddOrUpdate(csu2);
            }

            #endregion

            #region order statuses, change reasons

            if (context.OrderStatuses.Count() == 0)
            {
                OrderStatus status1 = new OrderStatus()
                {
                    Id = 1,
                    Name = "Open",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                OrderStatus status2 = new OrderStatus()
                {
                    Id = 2,
                    Name = "Pending",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                OrderStatus status3 = new OrderStatus()
                {
                    Id = 3,
                    Name = "Confirmed",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                OrderStatus status4 = new OrderStatus()
                {
                    Id = 4,
                    Name = "Delivered",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
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
                    Name = "Over order",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                OrderChangeReason changeReason2 = new OrderChangeReason()
                {
                    Id = 2,
                    Name = "Late arrival",
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
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
                    Name = "Bidgee 13",
                    CustomerSiteId = siteBaiadaHanwood.Id,
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                Farm farm2 = new Farm()
                {
                    Id = 2,
                    Name = "Coly 25",
                    CustomerSiteId = siteBaiadaHanwood.Id,
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                Farm farm3 = new Farm()
                {
                    Id = 3,
                    Name = "Avenues 31",
                    CustomerSiteId = siteBaiadaHanwood.Id,
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                Farm farm4 = new Farm()
                {
                    Id = 3,
                    Name = "Farm 75 - ProTen",
                    CustomerSiteId = siteBaiadaHanwood.Id,
                    CreatedBy = sysAdmin,
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
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
                        Name = $"0001",
                        FarmId = farm.Id,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo1Shed1 = new Silo()
                    {
                        Name = $"S1",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed1Farm1,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo2Shed1 = new Silo()
                    {
                        Name = $"S2",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed1Farm1,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Shed shed2Farm1 = new Shed()
                    {
                        Name = $"0002",
                        FarmId = farm.Id,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo1Shed2 = new Silo()
                    {
                        Name = $"S1",
                        Capacity = 20,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo2Shed2 = new Silo()
                    {
                        Name = $"S2",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreatedBy = sysAdmin,
                        CreationDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    Silo silo3Shed2 = new Silo()
                    {
                        Name = $"S3",
                        Capacity = 30,
                        Occupancy = 0,
                        Shed = shed2Farm1,
                        CreatedBy = sysAdmin,
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
