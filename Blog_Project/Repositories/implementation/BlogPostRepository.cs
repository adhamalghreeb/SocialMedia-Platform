using Blog_Project.CORE.Models.Domain;
using Blog_Project.EF.Data;
using Blog_Project.Repositories.Interface;

using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Repositories.implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly appDBcontext dbcontext;
        public BlogPostRepository(appDBcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbcontext.blogPosts.AddAsync(blogPost);
            await dbcontext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> Delete(Guid id)
        {
            var exsit = await dbcontext.blogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(exsit is not null)
            {
                dbcontext.blogPosts.Remove(exsit);
                await dbcontext.SaveChangesAsync();

                return exsit;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync() => await dbcontext.blogPosts.Include(x => x.Categories).ToListAsync();

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbcontext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task<BlogPost?> GetByUrlAsync(string url)
        {
            return await dbcontext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(c => c.UrlHandle == url);
        }

        public async Task<BlogPost?> Update(BlogPost blogPost)
        {
            var exsist = await dbcontext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (exsist is null)
            {
                return null;
            }

            dbcontext.Entry(exsist).CurrentValues.SetValues(blogPost);
            exsist.Categories = blogPost.Categories;
            dbcontext.SaveChanges();
            return blogPost;
        }
    }
}
