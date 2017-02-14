namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeFieldsFixing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventMessages", "CommentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventMessages", "SendMessageDate", c => c.DateTime());
            AddColumn("dbo.ProfileMessages", "SendMessageDate", c => c.DateTime());
            DropColumn("dbo.EventMessages", "DateTime");
            DropColumn("dbo.ProfileMessages", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProfileMessages", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventMessages", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.ProfileMessages", "SendMessageDate");
            DropColumn("dbo.EventMessages", "SendMessageDate");
            DropColumn("dbo.EventMessages", "CommentDate");
        }
    }
}
