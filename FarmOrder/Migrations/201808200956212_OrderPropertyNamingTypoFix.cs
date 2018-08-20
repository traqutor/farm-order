namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderPropertyNamingTypoFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TonsOrdered", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "TonesOrdered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "TonesOrdered", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "TonsOrdered");
        }
    }
}
