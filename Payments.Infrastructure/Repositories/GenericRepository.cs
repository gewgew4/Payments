using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;
using Payments.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Payments.Infrastructure.Repositories
{
    public class GenericRepository<T>(PbContext context) : IGenericRepository<T> where T : BaseEntity<T>
    {
        protected readonly PbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T> GetById(Guid id, bool tracking = true, params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async void Remove(Guid id)
        {
            var entity = await GetById(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public Task<List<T>> GetAll(params string[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                    int? top = null,
                                                    int? skip = null,
                                                    params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                query = query.Take(top.Value);
            }

            return await query.ToListAsync();
        }
    }
}
