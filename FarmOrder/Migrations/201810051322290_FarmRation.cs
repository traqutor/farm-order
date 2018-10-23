namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FarmRation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FarmRations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FarmId = c.Int(nullable: false),
                        RationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Farms", t => t.FarmId, cascadeDelete: false)
                .ForeignKey("dbo.Rations", t => t.RationId, cascadeDelete: false)
                .Index(t => t.FarmId)
                .Index(t => t.RationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FarmRations", "RationId", "dbo.Rations");
            DropForeignKey("dbo.FarmRations", "FarmId", "dbo.Farms");
            DropIndex("dbo.FarmRations", new[] { "RationId" });
            DropIndex("dbo.FarmRations", new[] { "FarmId" });
            DropTable("dbo.FarmRations");
        }
    }
}
