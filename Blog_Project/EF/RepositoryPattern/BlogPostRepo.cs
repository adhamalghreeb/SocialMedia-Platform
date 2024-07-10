using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.EF.Data;

using Microsoft.EntityFrameworkCore;

namespace Blog_Project.EF.RepositoryPattern
{
    public class BlogPostRepo : BaseRepository<BlogPost>, IBlogPost
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
