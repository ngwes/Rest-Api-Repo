using Microsoft.EntityFrameworkCore;
using Rest_Api_Repo.Infrastructure;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        public IUnitOfWork UnitOfWork => _context;
        private readonly DbSet<TEntity> _dbSet;

        private readonly DataContext _context;

        protected GenericRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected async virtual Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int skip = 0,
            int take = 0)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                return await query.Take(take).AsNoTracking().ToListAsync();
            }
            if (orderBy != null)
            {
                return await orderBy(query).AsNoTracking().ToListAsync();
            }
           
            return await query.AsNoTracking().ToListAsync();
        }

        protected async virtual Task<TEntity> GetByIdAsync(object id)
        {
            var item = await _dbSet.FindAsync(id);
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        protected virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        protected async virtual Task DeleteByIdAsync(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        protected virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        protected virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
