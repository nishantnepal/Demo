using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Core.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class

    {
        private readonly ILogger<TEntity> _logger;
        readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context, ILogger<TEntity> logger)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        internal DbSet<TEntity> DbSet => _dbSet;
        internal DbContext Context => _context;
        internal ILogger Logger => _logger;

        /// <summary>
        /// filter student => student.LastName == "Smith"
        /// order by q => q.OrderBy(s => s.LastName)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty).AsNoTracking();
                }


            if (orderBy != null)
            {
                return orderBy(query).AsNoTracking().ToListAsync();
            }
            else
            {
                return query.AsNoTracking().ToListAsync();
            }
        }

        public virtual Task<TEntity> GetById(object id)
        {
            return _dbSet.FindAsync(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            Context.Database.Log = msg => Logger?.LogInformation(2, msg);
            return _dbSet.Add(entity);
        }

        public virtual async void Delete(object id)
        {
            TEntity entityToDelete = await GetById(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            Context.Database.Log = msg => Logger?.LogInformation(2, msg);
            var entity = _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entity;
        }
    }
}
