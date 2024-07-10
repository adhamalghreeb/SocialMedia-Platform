using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;

namespace Blog_Project.CORE.@interface
{
    public interface ICategory : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 5
            );
    }
}
