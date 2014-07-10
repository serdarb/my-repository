using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using EntityFramework.BulkInsert.Extensions;

using MyRepository.Data;
using MyRepository.Entity;

namespace MyRepository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable
           where TEntity : BaseEntity
    {
        private readonly DbContext _context;

        public Repository(DbContext context = null)
        {
            _context = context ?? new MyDbContext();
        }

        public virtual TEntity Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.Entry(entity).State = EntityState.Added;
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(long id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Remove(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            var objects = _context.Set<TEntity>().Where(where).AsEnumerable();
            foreach (var item in objects)
            {
                _context.Set<TEntity>().Remove(item);
                _context.Entry(item).State = EntityState.Deleted;
            }
        }

        public TEntity SelectOne(long id, bool isNotCached = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entity = SelectAll(x => x.Id == id, isNotCached, includeProperties);
            return entity.FirstOrDefault();
        }

        public virtual TEntity SelectOne(Expression<Func<TEntity, bool>> where = null, bool isNotCached = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entity = SelectAll(where, isNotCached, includeProperties);
            return entity.FirstOrDefault();
        }

        public virtual IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> where = null, bool isNotCached = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = where != null ? _context.Set<TEntity>().Where(where)
                                      : _context.Set<TEntity>();

            foreach (var property in includeProperties)
            {
                items.Include(property);
            }

            if (isNotCached)
            {
                items = items.AsNoTracking();
            }

            return items;
        }


        public virtual bool Any(Expression<Func<TEntity, bool>> where)
        {
            return _context.Set<TEntity>().Any(where);
        }

        public virtual long Count(Expression<Func<TEntity, bool>> where)
        {
            return _context.Set<TEntity>().Count(where);
        }

        public virtual long Sum(Expression<Func<TEntity, long>> where)
        {
            return _context.Set<TEntity>().Sum(where);
        }

        /// <summary>
        /// expects SaveChanges() to complete
        /// </summary>
        /// <param name="entities"></param>
        public void BulkInsertSlow(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<TEntity>().Add(entity);
                _context.Entry(entity).State = EntityState.Added;
            }
        }

        /// <summary>
        /// calls SaveChanges()
        /// </summary>
        /// <param name="entities"></param>
        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            _context.BulkInsert(entities);
            _context.SaveChanges();
        }


        public virtual bool SaveChanges()
        {
            return 0 < _context.SaveChanges();
        }

        public void Dispose()
        {
            if (null != _context)
            {
                _context.Dispose();
            }
        }
    }
}
