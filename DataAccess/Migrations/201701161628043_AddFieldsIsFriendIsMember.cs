namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsIsFriendIsMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "IsFriend", c => c.Boolean());
            AddColumn("dbo.Profiles", "IsMember", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "IsMember");
            DropColumn("dbo.Profiles", "IsFriend");
        }
    }
}
