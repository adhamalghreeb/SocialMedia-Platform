using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.Interface;
using Blog_Project.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog_Project.Repositories.implementation
{
    public class CategoryRepo : BaseRepository<Category>, ICategory
    {
        private readonly appDBcontext appDBcontext;

        public CategoryRepo(appDBcontext appDBcontext) : base(appDBcontext)
        {
            this.appDBcontext = appDBcontext;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1, int? pageSize = 5)
        {
            // query
            var categories = appDBcontext.categories.AsQueryable();

            // filter
            if (string.IsNullOrWhiteSpace(query) == false)
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

    }
}
