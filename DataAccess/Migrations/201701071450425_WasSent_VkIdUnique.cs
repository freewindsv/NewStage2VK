namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WasSent_VkIdUnique : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventMessages", "WasSent", c => c.Boolean(nullable: false));
            CreateIndex("dbo.EventMessages", "VkId", unique: true);
            CreateIndex("dbo.Profiles", "VkId", unique: true);
            CreateIndex("dbo.Users", "VkId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "VkId" });
            DropIndex("dbo.Profiles", new[] { "VkId" });
            DropIndex("dbo.EventMessages", new[] { "VkId" });
            DropColumn("dbo.EventMessages", "WasSent");
        }
    }
}
