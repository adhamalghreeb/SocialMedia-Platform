using Blog_Project.CORE.Models.Domain;

namespace Blog_Project.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 5
            );

        Task<Category> GetByIdAsync(Guid id);
        Task<Category> Update(Category category);
        Task<Category> DeleteCategory(Guid id);
    }
}
