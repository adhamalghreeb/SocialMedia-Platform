using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog_Project.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<T> GetById(Guid id, string[] includes = null);
        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null, int? pageNumber = 1, int? pageSize = 5, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC");

        public Task<T> Add(T entity);

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        public Task<T> Update(T entity);

        public void Delete(T entity);

        public Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);



    }
}
