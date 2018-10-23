namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerLogoAndCss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Logo", c => c.String());
            AddColumn("dbo.Customers", "CssFilePath", c => c.String());
            AddColumn("dbo.AspNetUsers", "CssFilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CssFilePath");
            DropColumn("dbo.Customers", "CssFilePath");
            DropColumn("dbo.Customers", "Logo");
        }
    }
}
