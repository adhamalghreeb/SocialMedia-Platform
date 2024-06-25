using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Repositories.implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly appDBcontext dbcontext;

        public CategoryRepository(appDBcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbcontext.categories.AddAsync(category);
            await dbcontext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteCategory(Guid id)
        {
            var cate = await dbcontext.categories.FirstOrDefaultAsync(c => c.Id == id);
            if (cate is null) { 
                return null; 
            }
            
            dbcontext.categories.Remove(cate);
            return cate;

        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1, int? pageSize = 5)
        {
            // query
            var categories = dbcontext.categories.AsQueryable();

            // filter
            if(string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            // sort
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                // sort by name
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    categories = isAsc ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
                }

                // sort by url
                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
                }
            }



            // paginagtion
            var skipResult = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResult ?? 0).Take(pageSize ?? 5);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id) => await dbcontext.categories.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category?> Update(Category category)
        {
            var cat = await dbcontext.categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(cat is not null)
            {
                dbcontext.Entry(cat).CurrentValues.SetValues(category);
                dbcontext.SaveChanges();
                return category;
            }
            return null;

        }
    }
}
