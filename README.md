my-repository
=============

my way of implementing the repository pattern with EntityFramework

      interface IRepository<T>
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
	
  	  bool SaveChanges();
      } 




* you can find unit tests under "src\MyRepository.Tests"
* to create user entity table use the sql under "files" folder



project uses these packages
---------------------------

* EntityFramework.6.1.1
* EntityFramework.BulkInsert-ef6.6.0.2.8
* EntityFramework.MappingAPI.6.0.0.7
* NUnit.2.6.3



