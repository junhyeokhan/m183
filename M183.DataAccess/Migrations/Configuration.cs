namespace M183.DataAccess.Migrations
{
    using M183.DataAccess.Models.Configurations;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            List<GlobalConfiguration> globalConfigurations = new List<GlobalConfiguration>();

            GlobalConfiguration homeIntroductionConfiguration = new GlobalConfiguration()
            {
                Key = "Home_Introduction",
                Value = "Hello! This application is for module 183.",
            };

            globalConfigurations.Add(homeIntroductionConfiguration);

            context.GlobalConfigurations.AddRange(globalConfigurations);
        }
    }
}
