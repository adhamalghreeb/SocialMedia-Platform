using Blog_Project.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Data
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options) : base(options) { }
        public DbSet<BlogPost> blogPosts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<BlogImage> blogImages { get; set; }
    }
}
