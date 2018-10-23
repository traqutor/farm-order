namespace FarmOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserNotificationAndIsEmergencyFlagToOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        RecipientEmailAddress = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        ModifiedById = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedById)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedById)
                .Index(t => t.ModifiedById);
            
            AddColumn("dbo.Orders", "IsEmergency", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "ModifiedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.UserNotifications", new[] { "ModifiedById" });
            DropIndex("dbo.UserNotifications", new[] { "CreatedById" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropColumn("dbo.Orders", "IsEmergency");
            DropTable("dbo.UserNotifications");
        }
    }
}
