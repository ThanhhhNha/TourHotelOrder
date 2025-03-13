using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VietTravel.DataAccess
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string includeProperties = "");
        T GetById(int? id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
