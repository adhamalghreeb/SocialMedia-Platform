using Blog_Project.CORE.Models.Domain;
using Blog_Project.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata;

namespace Blog_Project.EF.Data
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options) : base(options) { }

        public DbSet<BlogPost> blogPosts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<BlogImage> blogImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<AppUser> appUsers { get; set; }
        public DbSet<follow> follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("AspNetUsers");

            modelBuilder.Entity<UserPermissions>()
                .HasKey(x => new { x.UserId, x.PermissionId });

            modelBuilder.Entity<UserPermissions>()
                .HasOne(up => up.User)
                .WithMany(u => u.userPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermissions>()
                    .Property(up => up.PermissionId)
                    .HasConversion<int>(); // Storing enum as int in the database

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(x => x.BlogPost)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.BlogPostId);

            modelBuilder.Entity<follow>()
                .HasKey(x => new { x.FollowerId, x.FolloweeId });

            modelBuilder.Entity<follow>()
                .HasOne(f => f.Follower)
                .WithMany(up => up.Followees)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<follow>()
                .HasOne(f => f.Followee)
                .WithMany(up => up.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
