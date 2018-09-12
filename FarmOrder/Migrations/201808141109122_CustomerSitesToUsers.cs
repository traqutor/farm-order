namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerSitesToUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerSiteUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerSiteId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerSites", t => t.CustomerSiteId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CustomerSiteId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerSiteUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomerSiteUsers", "CustomerSiteId", "dbo.CustomerSites");
            DropIndex("dbo.CustomerSiteUsers", new[] { "UserId" });
            DropIndex("dbo.CustomerSiteUsers", new[] { "CustomerSiteId" });
            DropTable("dbo.CustomerSiteUsers");
        }
    }
}
