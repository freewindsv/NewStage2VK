namespace NewStage2VK.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NewStage2VK.DataAccess.DataModel.NewStage2DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            #if DEBUG
            AppDomain.CurrentDomain.SetData("DataDirectory", System.Configuration.ConfigurationManager.AppSettings["efMigrationDataDirectory"]);
            #endif
        }

        protected override void Seed(NewStage2VK.DataAccess.DataModel.NewStage2DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
