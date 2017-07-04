using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SeedApp.Common.Entities;

namespace SeedApp.Common.Interfaces
{
	public interface IAppInternalDatabase
	{
		T GetSingleOrDefaultByEntityKey<T>(string entityKey)
			where T : EntityBase;

		T GetSingleOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class;

		T GetFirstOrDefaultByQuery<T>(Expression<Func<T, bool>> expression) where T : class;

		List<T> LoadMany<T>(Expression<Func<T, bool>> expression = null)
			where T : class;

		List<T> LoadMany<T, TValue>(Expression<Func<T, bool>> expression = null,
									Expression<Func<T, TValue>> orderBy = null, int? skip = null, int? take = null) where T : class;

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

		List<T> Execute<T>(string query, params object[] args)
			where T : EntityBase;

		void UpdateAll<T>(IEnumerable<T> entities)
			where T : class;

		T GetFirstOrDefaultByQuery<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null) where T : EntityBase;

		IDisposable Lock();
	}
}
