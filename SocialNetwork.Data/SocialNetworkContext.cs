namespace SocialNetwork.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SocialNetworkContext : IdentityDbContext<ApplicationUser>
    {
       
        public SocialNetworkContext()
            : base("name=SocialNetwork.Context")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SocialNetworkContext, Configuration>());
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<PostLike> PostLikes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.WallPosts)
                .WithRequired(p => p.WallOwner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.OwnPosts)
                .WithRequired(p => p.Author)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


        public static SocialNetworkContext Create()
        {
            return new SocialNetworkContext();
        }

    }

    
}