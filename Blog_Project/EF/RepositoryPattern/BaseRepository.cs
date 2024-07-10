using Blog_Project.CORE.@interface;
using Blog_Project.EF.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog_Project.EF.RepositoryPattern
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly appDBcontext appDBcontext;

        public BaseRepository(appDBcontext appDBcontext)
        {
            this.appDBcontext = appDBcontext;
        }

        public async Task<T> Add(T entity)
        {
            await appDBcontext.Set<T>().AddAsync(entity);
            return entity;

        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await appDBcontext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            appDBcontext.Update(entity);
            return entity;
        }

        public async void Delete(T entity)
        {
            appDBcontext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null, int? pageNumber = 1, int? pageSize = 5, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC")
        {
            IQueryable<T> query = appDBcontext.Set<T>().Where(criteria);

            // includes all
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            // sort
            if (orderBy != null)
            {
                if (orderByDirection == orderByDirection)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            // paginagtion
            var skipResult = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResult ?? 0).Take(pageSize ?? 5);

            return await query.ToListAsync();
        }

        public async Task<T> GetById(Guid id, string[] includes = null)
        {
            IQueryable<T> query = appDBcontext.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            // Use reflection to access the Id property
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(property, value);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = appDBcontext.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = appDBcontext.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync(criteria);
        }



    }
}
