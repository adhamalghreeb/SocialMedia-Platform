using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;

namespace Blog_Project.CORE.@interface
{
    public interface IBlogPost : IBaseRepository<BlogPost>
    {
        Task<BlogPost?> GetByUrlAsync(string url);
    }
}
