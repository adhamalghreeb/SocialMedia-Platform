using Blog_Project.Models.Domain;

namespace Blog_Project.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetByIdAsync(Guid id);
        Task<BlogPost?> Update(BlogPost blogPost);
        Task<BlogPost?> Delete(Guid id);
        Task<BlogPost?> GetByUrlAsync(string url);
    }
}
