using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SeedApp.Common.Data;
using SeedApp.Common.Models;
using SQLite.Net;

namespace SeedApp.Data
{
    public abstract class Database : IDatabase
    {
        protected SQLiteConnectionWithLock Connection;

        public IDisposable Lock()
        {
            return Connection.Lock();
        }

        public T GetSingleOrDefaultById<T>(int id)
            where T : EntityBase
        {
            using (Connection.Lock())
            {
                return Connection.Find<T>(x => x.Id == id);
            }
        }

        public T GetSingleOrDefaultByQuery<T>(Expression<Func<T, bool>> expression)
            where T : class
        {
            return LoadMany(expression).SingleOrDefault();
        }

        public int Count<T>(Expression<Func<T, bool>> expression = null)
            where T : class
        {
            using (Connection.Lock())
            {
                return expression != null ? Connection.Table<T>().Where(expression).Count()
                    : Connection.Table<T>().Count();
            }
        }

        public List<T> LoadMany<T>(Expression<Func<T, bool>> expression = null) where T : class
        {
            using (Connection.Lock())
            {
                return expression != null ? Connection.Table<T>().Where(expression).ToList() : Connection.Table<T>().ToList();
            }
        }

        public void Insert<T>(T entity)
            where T : class
        {
            if (entity is EntityBase)
            {
                PrepareForInsert(entity as EntityBase);
            }

            using (Connection.Lock())
            {
                Connection.Insert(entity);
            }
        }

        public void InsertAll<T>(IList<T> entityList) where T : EntityBase
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entity in entityList)
            {
                PrepareForInsert<T>(entity, utcNow, utcNow);
            }

            using (Connection.Lock())
            {
                Connection.InsertAll(entityList);
            }
        }

        public void Update<T>(T entity)
            where T : class
        {
            if (entity is EntityBase)
            {
                PrepareForUpdate(entity as EntityBase);
            }

            using (Connection.Lock())
            {
                Connection.Update(entity);
            }
        }

        public void UpdateAll<T>(IEnumerable<T> entities) where T : class
        {
            var enumerable = entities as T[] ?? entities.ToArray();

            var utcNow = DateTime.UtcNow;

            foreach (var entity in enumerable.Where(x => x is EntityBase).Cast<EntityBase>())
            {
                PrepareForUpdate(entity, utcNow);
            }

            using (Connection.Lock())
            {
                Connection.UpdateAll(enumerable);
            }
        }

        public void InsertOrUpdate<T>(T entity)
            where T : EntityBase
        {
            T existing = GetSingleOrDefaultById<T>(entity.Id);

            if (existing == null)
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }
        }

        public void Delete<T>(T entity) where T : EntityBase
        {
            using (Connection.Lock())
            {
                Connection.Delete(entity);
            }
        }

        public void DeleteAll<T>()
        {
            using (Connection.Lock())
            {
                Connection.DeleteAll<T>();
            }
        }

        public void RunInTransaction(Action inTransaction)
        {
            using (Connection.Lock())
            {
                Connection.RunInTransaction(inTransaction);
            }
        }

        public void Execute(string query, params object[] args)
        {
            using (Connection.Lock())
            {
                Connection.Execute(query, args);
            }
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public T GetFirstOrDefaultByQuery<T, TKey>(Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> orderBy = null) where T : EntityBase
        {
            using (Connection.Lock())
            {
                return Connection.Table<T>().Where(expression).OrderBy(orderBy).FirstOrDefault();
            }
        }

        private void PrepareForInsert<T>(T entity, DateTime? clientModifiedOnUtc = null, DateTime? clientCreatedOnUtc = null) where T : EntityBase
        {
            var utcNow = DateTime.UtcNow;

            if (clientModifiedOnUtc == null)
            {
                clientModifiedOnUtc = utcNow;
            }

            if (clientCreatedOnUtc == null)
            {
                clientCreatedOnUtc = utcNow;
            }

            entity.ClientCreatedOnUtc = clientCreatedOnUtc.Value;
            entity.ClientModifiedOnUtc = clientModifiedOnUtc.Value;
        }

        private void PrepareForUpdate<T>(T entity, DateTime? clientModifiedOnUtc = null) where T : EntityBase
        {
            entity.ClientModifiedOnUtc = clientModifiedOnUtc ?? DateTime.UtcNow;
        }

        public List<T> Query<T>(string query, params object[] args) where T : class
        {
            using (Connection.Lock())
            {
                return Connection.Query<T>(query, args);
            }
        }
    }
}