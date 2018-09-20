namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderRation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerSiteId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerSites", t => t.CustomerSiteId, cascadeDelete: false)
                .Index(t => t.CustomerSiteId);
            
            AddColumn("dbo.Orders", "RationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "RationId");
            AddForeignKey("dbo.Orders", "RationId", "dbo.Rations", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "RationId", "dbo.Rations");
            DropForeignKey("dbo.Rations", "CustomerSiteId", "dbo.CustomerSites");
            DropIndex("dbo.Rations", new[] { "CustomerSiteId" });
            DropIndex("dbo.Orders", new[] { "RationId" });
            DropColumn("dbo.Orders", "RationId");
            DropTable("dbo.Rations");
        }
    }
}
