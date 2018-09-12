namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FarmOrderEtc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Farms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomerSiteId = c.Int(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerSites", t => t.CustomerSiteId, cascadeDelete: true)
                .Index(t => t.CustomerSiteId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        TonesOrdered = c.Int(nullable: false),
                        FarmId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ChangeReasonId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderChangeReasons", t => t.ChangeReasonId)
                .ForeignKey("dbo.Farms", t => t.FarmId, cascadeDelete: true)
                .ForeignKey("dbo.OrderStatus", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.FarmId)
                .Index(t => t.StatusId)
                .Index(t => t.ChangeReasonId);
            
            CreateTable(
                "dbo.OrderChangeReasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "StatusId", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "FarmId", "dbo.Farms");
            DropForeignKey("dbo.Orders", "ChangeReasonId", "dbo.OrderChangeReasons");
            DropForeignKey("dbo.Farms", "CustomerSiteId", "dbo.CustomerSites");
            DropIndex("dbo.Orders", new[] { "ChangeReasonId" });
            DropIndex("dbo.Orders", new[] { "StatusId" });
            DropIndex("dbo.Orders", new[] { "FarmId" });
            DropIndex("dbo.Farms", new[] { "CustomerSiteId" });
            DropTable("dbo.OrderStatus");
            DropTable("dbo.OrderChangeReasons");
            DropTable("dbo.Orders");
            DropTable("dbo.Farms");
        }
    }
}
