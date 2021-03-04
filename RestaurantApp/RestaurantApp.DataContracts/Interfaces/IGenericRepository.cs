using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.DataContracts.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params string[] include);
        bool Save(T entity);
        bool Add(T entity);
        bool AddRange(IEnumerable<T> entity);
        bool RemoveRange(IEnumerable<T> entities);
        bool Save(IEnumerable<T> entity);
        bool Update(T entity);
        bool DeleteById(object id);
        bool Delete(T entity);
        void DeleteAll();
    }
}
