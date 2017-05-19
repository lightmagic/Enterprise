namespace NewsMedia.Migrations
{
    using NewsMedia.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsMedia.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NewsMedia.Models.UsersContext context)
        {
            context.UserProfiles.AddOrUpdate(r => r.UserName,
            new UserProfile { UserName = "kyle354", firstName = "kyle", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" },
            new UserProfile { UserName = "kyle", firstName = "kyle354", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" }
            );
            //
        }
    }
}
