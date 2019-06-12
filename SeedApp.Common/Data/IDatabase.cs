using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SeedApp.Common.Models;

namespace SeedApp.Common.Data
{
    public interface IDatabase
    {
        T GetSingleOrDefaultById<T>(int id)
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

        void RunInTransaction(Action inTransaction);

        void DeleteAll<T>();

        void Execute(string query, params object[] args);

        List<T> Query<T>(string query, params object[] args) where T : class;

        void UpdateAll<T>(IEnumerable<T> entities)
            where T : class;

        IDisposable Lock();
    }

    public interface ILogDatabase : IDatabase
    {
    }

    public interface IAppDatabase : IDatabase
    {
    }

    public interface IContactsDatabase : IDatabase
    {
    }
}