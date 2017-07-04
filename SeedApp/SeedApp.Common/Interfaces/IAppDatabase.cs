using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SeedApp.Common.Entities;

namespace SeedApp.Common.Interfaces
{
	public interface IAppDatabase
	{
		T GetSingleOrDefaultByEntityKey<T>(string entityKey)
		  where T : EntityBase;

		T GetSingleOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class;

		List<T> LoadMany<T>(Expression<Func<T, bool>> expression = null)
			where T : class;

		int Count<T>(Expression<Func<T, bool>> expression = null) where T : class;

		void Insert<T>(T entity)
			where T : class;

		void InsertAll<T>(IList<T> entityList) where T : EntityBase;

		void Update<T>(T entity)
					where T : class;

		void InsertOrUpdate<T>(T entity)
			where T : EntityBase;

		void Delete<T>(T entity)
			where T : EntityBase;

		void DeleteAllExceptNonSynced<T>()
			where T : EntityBase;

		void RunInTransaction(Action inTransaction);

		void DeleteAll<T>();

		void Execute(string query, params object[] args);

		void UpdateAll<T>(IEnumerable<T> entities)
			where T : class;

		Task InsertAsync<T>(T entity)
			where T : class;

		Task ExecuteAsync(string query, params object[] args);

		Task<List<T>> ExecuteAsync<T>(string query, params string[] args) where T : EntityBase;

		Task UpdateAllAsync<T>(IEnumerable<T> entities)
		where T : class;

		Task<int> CountAsync<T>(Expression<Func<T, bool>> expression = null) where T : class;

		Task UpdateAsync<T>(T entity)
		where T : class;

		Task DeleteAsync<T>(T entity)
			where T : EntityBase;

		Task<T> GetSingleOrDefaultByQueryAsync<T>(Expression<Func<T, bool>> expression)
		where T : EntityBase;

		Task<T> GetFirstOrDefaultByQueryAsync<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null)
		where T : EntityBase;

		Task RunInTransactionAsync(Action inTransaction);

		Task<List<T>> LoadManyAsync<T>(Expression<Func<T, bool>> expression = null) where T : class;

		Task<List<T>> LoadManyAsync<T, TValue>(Expression<Func<T, bool>> expression = null,
									Expression<Func<T, TValue>> orderBy = null, int? skip = null, int? take = null) where T : class;

		Task<T> GetSingleOrDefaultByEntityKeyAsync<T>(string entityKey)
		where T : EntityBase;

		Task InsertOrUpdateAsync<T>(T entity)
		where T : EntityBase;

		Task DeleteAllAsync<T>();

		T GetFirstOrDefaultByQuery<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null) where T : EntityBase;

		T GetFirstOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class;

		Task<T> GetFirstOrDefaultByQueryAsync<T>(Expression<Func<T, bool>> expression) where T : class;

		Task InsertAllAsync<T>(IList<T> entityList) where T : EntityBase;

		IDisposable Lock();
	}
}
