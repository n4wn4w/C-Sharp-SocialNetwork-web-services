namespace SocialNetwork.Data.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SocialNetwork.Data.SocialNetworkContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SocialNetwork.Data.SocialNetworkContext";
            

        }

        protected override void Seed(SocialNetworkContext context)
        {
           if (context.Roles.Any())
           {
               return;
           }
             
            var user = context.Users.First();

            var adminRole = new IdentityRole()
            {
                Name = "Admin"
            };

            var moderatorRole = new IdentityRole()
            {
                Name = "Moderator"
            };

            context.Roles.Add(adminRole);
            context.Roles.Add(moderatorRole);
            context.SaveChanges();

            adminRole.Users.Add(new IdentityUserRole()
            {
                UserId = user.Id
            });

            context.SaveChanges();
        }
    }
}
