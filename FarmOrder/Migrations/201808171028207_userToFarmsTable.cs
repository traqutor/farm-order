namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userToFarmsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FarmUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FarmId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Farms", t => t.FarmId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.FarmId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FarmUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FarmUsers", "FarmId", "dbo.Farms");
            DropIndex("dbo.FarmUsers", new[] { "UserId" });
            DropIndex("dbo.FarmUsers", new[] { "FarmId" });
            DropTable("dbo.FarmUsers");
        }
    }
}
