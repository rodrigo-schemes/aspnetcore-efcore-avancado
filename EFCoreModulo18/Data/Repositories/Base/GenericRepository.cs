using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCoreModulo18.Data.Repositories.Base
{
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T>
        where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
           _dbSet.Update(entity);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.CountAsync(expression);
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetDataAsync(
            Expression<Func<T, bool>>? expression = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
            int? skip = null, 
            int? take = null)
        {
            var query = _dbSet.AsQueryable();

            if(expression != null)
            {
                query = query.Where(expression);
            }

            if(include != null)
            {
                query = include(query);
            }

            if(skip is not null)
            {
                query = query.Skip(skip.Value);
            }

            if(take is not null)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }
    }
}