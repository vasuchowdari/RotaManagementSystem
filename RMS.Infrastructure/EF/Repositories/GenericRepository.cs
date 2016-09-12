using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RMS.Infrastructure.EF.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal RmsContext _ctx;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(RmsContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<TEntity>();
            //_ctx.Configuration.AutoDetectChangesEnabled = true;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void CreateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        //public virtual void UpdateRange(IEnumerable<TEntity> entities)
        //{

        //}

        public virtual void Delete(object id)
        {
            TEntity entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_ctx.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }
    }
}
