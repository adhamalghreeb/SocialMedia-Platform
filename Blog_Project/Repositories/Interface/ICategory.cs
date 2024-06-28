using Blog_Project.Models.Domain;
using Blog_Project.Repository;

namespace Blog_Project.Repositories.Interface
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
