namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldAllowPrivateMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "AllowPrivateMessages", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "AllowPrivateMessages");
        }
    }
}
