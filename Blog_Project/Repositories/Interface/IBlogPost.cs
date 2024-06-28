using Blog_Project.Models.Domain;
using Blog_Project.Repository;

namespace Blog_Project.Repositories.Interface
{
    public interface IBlogPost : IBaseRepository<BlogPost>
    {
        Task<BlogPost?> GetByUrlAsync(string url);
    }
}
