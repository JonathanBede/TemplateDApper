using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tornado.Domain.Interfaces;

namespace Tornado.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        IEnumerable<T> GetAll();

        int GetTotal();

        T Get(Guid id);

        ICollection<T> Where(Expression<Func<T, bool>> match);

        T Create(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Delete(Guid id);

        void DeleteAll();
        
    }
}
