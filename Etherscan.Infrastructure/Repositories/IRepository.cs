using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Etherscan.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Get(int id);
        void Update(T entity);
        void Delete(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T,bool>> predicate);
        void SaveChanges();
    }
}