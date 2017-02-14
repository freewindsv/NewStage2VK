namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldPresenceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "PresenceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "PresenceType");
        }
    }
}
