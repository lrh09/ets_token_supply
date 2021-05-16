using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Etherscan.Infrastructure.Repositories
{
    public abstract class GenericRepository<T>: IRepository<T> where T: class
    {
        protected EtherscanContext _context;

        public GenericRepository(EtherscanContext context)
        {
            _context = context;
        }

        public virtual T Add(T entity)
        {
            return _context.Set<T>().Add(entity).Entity;
        }

        public virtual T Get(int id)
        {
            return _context.Find<T>(id);
        }
        
        public void Update(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            T entity = Get(id);
            _context.Set<T>().Remove(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T,bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }
        
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}