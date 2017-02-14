namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldProfileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "Type");
        }
    }
}
