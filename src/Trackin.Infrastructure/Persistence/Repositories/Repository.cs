
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TrackinContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(TrackinContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize, string? ordering = null, bool descendingOrder = false)
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrEmpty(ordering))
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression property = Expression.Property(parameter, ordering);
                LambdaExpression lambda = Expression.Lambda(property, parameter);
                string methodName = descendingOrder ? "OrderByDescending" : "OrderBy";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.Type);
                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
            }
            int totalCount = await query.CountAsync();
            List<T> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
