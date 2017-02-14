namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProfileMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        IsSend = c.Boolean(nullable: false),
                        WasSent = c.Boolean(nullable: false),
                        Message_Id = c.Int(nullable: false),
                        Profile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id, cascadeDelete: true)
                .Index(t => t.Message_Id)
                .Index(t => t.Profile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileMessages", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.ProfileMessages", "Message_Id", "dbo.Messages");
            DropIndex("dbo.ProfileMessages", new[] { "Profile_Id" });
            DropIndex("dbo.ProfileMessages", new[] { "Message_Id" });
            DropTable("dbo.ProfileMessages");
            DropTable("dbo.Messages");
        }
    }
}
