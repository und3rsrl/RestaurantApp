using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.DataServices.Implementation
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly Entities DbContext;
        protected abstract string TableName { get; }

        protected GenericRepository(Entities dbContext)
        {
            DbContext = dbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public T GetById(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params string[] include)
        {
            IQueryable<T> query = DbContext.Set<T>();
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public bool Save(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return true;
        }

        public bool Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return true;
        }

        public bool AddRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
            return true;
        }

        public bool RemoveRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
            return true;
        }

        public bool Save(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Save(entity);
            }
            return true;
        }

        public bool Update(T entity)
        {
            DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool DeleteById(object id)
        {
            T entity = GetById(id);
            if (entity != null)
            {
                DbContext.Set<T>().Remove(entity);
                return true;
            }
            return false;
        }

        public bool Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Set<T>().Attach(entity);
            }
            DbContext.Set<T>().Remove(entity);
            return true;
        }

        public void DeleteAll()
        {
            DbContext.Database.ExecuteSqlRaw($"DELETE FROM [{TableName}]");
        }
    }
}
