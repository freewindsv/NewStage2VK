namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldProfileSex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "Sex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "Sex");
        }
    }
}
