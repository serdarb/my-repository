using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MyRepository.Entity;

namespace MyRepository.Repository
{
    public interface IRepository
    {
        bool SaveChanges();
    }

    public interface IRepository<T> : IRepository
           where T : BaseEntity
    {
        T Insert(T entity);
        T Update(T entity);

        void Delete(long id);
        void Delete(Expression<Func<T, bool>> where);

        T SelectOne(long id, bool isNotCached = false, params Expression<Func<T, object>>[] includeProperties);
        T SelectOne(Expression<Func<T, bool>> where = null, bool isNotCached = false, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> SelectAll(Expression<Func<T, bool>> where = null, bool isNotCached = false, params Expression<Func<T, object>>[] includeProperties);

        bool Any(Expression<Func<T, bool>> where);
        long Count(Expression<Func<T, bool>> where);
        long Sum(Expression<Func<T, long>> where);

        void BulkInsertSlow(IEnumerable<T> entities);
        void BulkInsert(IEnumerable<T> entities);
    }
}