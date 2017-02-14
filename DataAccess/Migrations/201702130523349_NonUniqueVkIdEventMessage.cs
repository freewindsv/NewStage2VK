namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonUniqueVkIdEventMessage : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EventMessages", new[] { "VkId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.EventMessages", "VkId", unique: true);
        }
    }
}
