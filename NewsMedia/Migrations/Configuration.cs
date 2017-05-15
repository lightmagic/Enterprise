namespace NewsMedia.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using NewsMedia.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsMedia.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NewsMedia.Models.UsersContext context)
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

            //by overriding the seed method, it checks if the data is in the database. If it isn't, then create it, if it is, then update
            context.UserProfiles.AddOrUpdate(r => r.UserName,
            new UserProfile { UserName = "kyle354", firstName = "kyle",    lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" },
            new UserProfile { UserName = "kyle",    firstName = "kyle354", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" },
            new UserProfile { UserName = "kyle354", firstName = "kyle", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" },
            new UserProfile { UserName = "kyle354", firstName = "kyle", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" },
            new UserProfile { UserName = "kyle354", firstName = "kyle", lastName = "schem", email = "kyleschem@mail.com", profileDesc = "Testing Seed" }
            );
        }
    }
}
