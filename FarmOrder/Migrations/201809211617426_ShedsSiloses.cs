namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShedsSiloses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderSiloes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        SiloId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .ForeignKey("dbo.Silos", t => t.SiloId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.SiloId);
            
            CreateTable(
                "dbo.Silos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Capacity = c.Int(nullable: false),
                        Occupancy = c.Int(nullable: false),
                        ShedId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sheds", t => t.ShedId, cascadeDelete: false)
                .Index(t => t.ShedId);
            
            CreateTable(
                "dbo.Sheds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FarmId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Farms", t => t.FarmId, cascadeDelete: false)
                .Index(t => t.FarmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderSiloes", "SiloId", "dbo.Silos");
            DropForeignKey("dbo.Silos", "ShedId", "dbo.Sheds");
            DropForeignKey("dbo.Sheds", "FarmId", "dbo.Farms");
            DropForeignKey("dbo.OrderSiloes", "OrderId", "dbo.Orders");
            DropIndex("dbo.Sheds", new[] { "FarmId" });
            DropIndex("dbo.Silos", new[] { "ShedId" });
            DropIndex("dbo.OrderSiloes", new[] { "SiloId" });
            DropIndex("dbo.OrderSiloes", new[] { "OrderId" });
            DropTable("dbo.Sheds");
            DropTable("dbo.Silos");
            DropTable("dbo.OrderSiloes");
        }
    }
}
