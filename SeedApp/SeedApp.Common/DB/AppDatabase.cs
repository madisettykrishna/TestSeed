using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SeedApp.Common.Entities;
using SeedApp.Common.Interfaces;

namespace SeedApp.Common.DB
{
	public class AppDatabase : IAppDatabase
	{
		private IAppInternalDatabase _database;

		protected AppDatabase(IAppInternalDatabase database)
		{
			_database = database;
		}

		public IDisposable Lock()
		{
			return _database.Lock();
		}

		public int Count<T>(Expression<Func<T, bool>> expression = null) where T : class
		{
			return _database.Count<T>(expression);
		}

		public Task<int> CountAsync<T>(Expression<Func<T, bool>> expression = null) where T : class
		{
			return Task.Factory.StartNew(() =>
			{
				return Count(expression);
			});
		}

		public void Delete<T>(T entity) where T : EntityBase
		{
			_database.Delete(entity);
		}

		public void DeleteAll<T>()
		{
			_database.DeleteAll<T>();
		}

		public Task DeleteAllAsync<T>()
		{
			return Task.Factory.StartNew(() =>
			{
				DeleteAll<T>();
			});
		}

		public void DeleteAllExceptNonSynced<T>() where T : EntityBase
		{
			_database.DeleteAllExceptNonSynced<T>();
		}

		public Task DeleteAsync<T>(T entity) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			{
				Delete(entity);
			});
		}

		public void Execute(string query, params object[] args)
		{
			_database.Execute(query, args);
		}

		public Task ExecuteAsync(string query, params object[] args)
		{
			return Task.Factory.StartNew(() =>
			 {
				 Execute(query, args);
			 });
		}

		public Task<List<T>> ExecuteAsync<T>(string query, params string[] args) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			 {
				 return _database.Execute<T>(query, args);
			 });
		}

		public Task<T> GetFirstOrDefaultByQueryAsync<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			 {
				 return GetFirstOrDefaultByQuery(expression, orderBy);
			 });
		}

		public T GetFirstOrDefaultByQuery<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null) where T : EntityBase
		{
			return _database.GetFirstOrDefaultByQuery(expression, orderBy);
		}

		public T GetSingleOrDefaultByEntityKey<T>(string entityKey) where T : EntityBase
		{
			return _database.GetSingleOrDefaultByEntityKey<T>(entityKey);
		}

		public Task<T> GetSingleOrDefaultByEntityKeyAsync<T>(string entityKey) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			{
				return GetSingleOrDefaultByEntityKey<T>(entityKey);
			});
		}

		public T GetSingleOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class
		{
			return _database.GetSingleOrDefaultByQuery<T>(expression);
		}

		public Task<T> GetSingleOrDefaultByQueryAsync<T>(Expression<Func<T, bool>> expression) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			{
				return GetSingleOrDefaultByQuery<T>(expression);
			});
		}

		public T GetFirstOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class
		{
			return _database.GetFirstOrDefaultByQuery<T>(expression);
		}

		public Task<T> GetFirstOrDefaultByQueryAsync<T>(Expression<Func<T, bool>> expression) where T : class
		{
			return Task.Factory.StartNew(() =>
			{
				return _database.GetFirstOrDefaultByQuery<T>(expression);
			});
		}

		public void Insert<T>(T entity) where T : class
		{
			_database.Insert(entity);
		}

		public void InsertAll<T>(IList<T> entityList) where T : EntityBase
		{
			_database.InsertAll(entityList);
		}

		public Task InsertAsync<T>(T entity) where T : class
		{
			return Task.Factory.StartNew(() =>
			  {
				  Insert(entity);
			  });
		}

		public void InsertOrUpdate<T>(T entity) where T : EntityBase
		{
			_database.InsertOrUpdate(entity);
		}

		public Task InsertOrUpdateAsync<T>(T entity) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
			  {
				  InsertOrUpdate(entity);
			  });
		}

		public List<T> LoadMany<T>(Expression<Func<T, bool>> expression = null) where T : class
		{
			return _database.LoadMany<T>(expression);
		}

		public Task<List<T>> LoadManyAsync<T>(Expression<Func<T, bool>> expression = null) where T : class
		{
			return Task.Factory.StartNew(() =>
			  {
				  return LoadMany<T>(expression);
			  });
		}

		public Task<List<T>> LoadManyAsync<T, TValue>(Expression<Func<T, bool>> expression = null,
								 Expression<Func<T, TValue>> orderBy = null, int? skip = null, int? take = null) where T : class
		{
			return Task.Factory.StartNew(() =>
			 {
				 return _database.LoadMany<T, TValue>(expression, orderBy, skip, take);
			 });
		}

		public void RunInTransaction(Action inTransaction)
		{
			_database.RunInTransaction(inTransaction);
		}

		public Task RunInTransactionAsync(Action inTransaction)
		{
			return Task.Factory.StartNew(() =>
			{
				RunInTransaction(inTransaction);
			});
		}

		public void Update<T>(T entity) where T : class
		{
			_database.Update(entity);
		}

		public void UpdateAll<T>(IEnumerable<T> entities) where T : class
		{
			_database.UpdateAll(entities);
		}

		public Task UpdateAllAsync<T>(IEnumerable<T> entities) where T : class
		{
			return Task.Factory.StartNew(() =>
		   {
			   UpdateAll(entities);
		   });
		}

		public Task UpdateAsync<T>(T entity) where T : class
		{
			return Task.Factory.StartNew(() =>
		  {
			  Update(entity);
		  });
		}

		public Task InsertAllAsync<T>(IList<T> entityList) where T : EntityBase
		{
			return Task.Factory.StartNew(() =>
		  {
			  InsertAll(entityList);
		  });
		}
	}
}
