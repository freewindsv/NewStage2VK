namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        Message = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VkId = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 1024),
                        DateTime = c.DateTime(nullable: false),
                        IsSend = c.Boolean(nullable: false),
                        Event_Id = c.Int(nullable: false),
                        Profile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VkId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Avatar_50 = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CurrentUser_Id)
                .Index(t => t.CurrentUser_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VkId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 256),
                        LastName = c.String(nullable: false, maxLength: 256),
                        Avatar_50 = c.Binary(),
                        EncryptedAccessToken = c.String(maxLength: 1024),
                        ExpireIn = c.Int(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Settings", "CurrentUser_Id", "dbo.Users");
            DropForeignKey("dbo.EventMessages", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.EventMessages", "Event_Id", "dbo.Events");
            DropIndex("dbo.Settings", new[] { "CurrentUser_Id" });
            DropIndex("dbo.EventMessages", new[] { "Profile_Id" });
            DropIndex("dbo.EventMessages", new[] { "Event_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Settings");
            DropTable("dbo.Profiles");
            DropTable("dbo.EventMessages");
            DropTable("dbo.Events");
        }
    }
}
