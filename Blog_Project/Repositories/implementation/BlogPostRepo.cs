using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.Interface;
using Blog_Project.Repository;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Repositories.implementation
{
    public class BlogPostRepo : BaseRepository<BlogPost> , IBlogPost
    {
        private readonly appDBcontext appDBcontext;

        public BlogPostRepo(appDBcontext appDBcontext) : base(appDBcontext)
        {
            this.appDBcontext = appDBcontext;
        }

        public async Task<BlogPost?> GetByUrlAsync(string url)
        {
            return await appDBcontext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(c => c.UrlHandle == url);
        }

    }
}
