namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderSiloAssignedAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderSiloes", "Amount", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderSiloes", "Amount");
        }
    }
}
