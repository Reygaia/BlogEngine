using BlogEngineClone.Areas.Identity.Data;
using Libs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BlogEngineClone.Data;

public class BlogEngineCloneContext : IdentityDbContext<BlogEngineCloneUser>
{
    public BlogEngineCloneContext(DbContextOptions<BlogEngineCloneContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Post { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Follow> Follow { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<CommentDetail> CommentDetail { get; set; }
    public DbSet<PostDetail> PostDetail { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Post)
            .HasForeignKey(p => p.UserID)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comment)
            .HasForeignKey(c => c.UserID)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowerID)
            .OnDelete(DeleteBehavior.Restrict); // You may adjust the delete behavior based on your requirements

        builder.Entity<Follow>()
            .HasOne(f => f.Following)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowingID)
            .OnDelete(DeleteBehavior.Restrict); // You may adjust the delete behavior based on your requirements
    }
}
