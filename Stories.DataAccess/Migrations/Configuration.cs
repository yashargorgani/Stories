namespace Stories.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Stories.DataAccess.StoriesDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Stories.DataAccess.StoriesDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.STR_StoryStatus.Any())
            {
                context.STR_StoryStatus.AddOrUpdate(
                        new Entities.STR_StoryStatus
                        {
                            Id = 1,
                            Caption = "چرک نویس",
                            Value = "Draft"
                        },
                        new Entities.STR_StoryStatus
                        {
                            Id = 2,
                            Caption = "منتشر",
                            Value = "Publish"
                        }
                    );
            }

            if (!context.STR_RateSubject.Any())
            {
                context.STR_RateSubject.AddOrUpdate(
                        new Entities.STR_RateSubject { Id = 1, Caption = "امتیاز" }
                    );
            }

            if (!context.STR_Topic.Any())
            {
                context.STR_Topic.AddOrUpdate(
                        new Entities.STR_Topic { Id = 1, Caption = "کامپیوتر", },
                        new Entities.STR_Topic { Id = 2, Caption = "هنر" });
            }


        }
    }
}
